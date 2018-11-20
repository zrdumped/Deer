using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
	public enum KeyStatus { Inactive, OnShow, Active };
	[Header("VFX")]
	//the vfx shown normally
	public GameObject InactiveBall;
    public GameObject InactiveBall_2;
	//the vfx shown after pressing E
	public GameObject HintBall;
	//the vfx shown after all matching
	public GameObject ActiveBall;
	//the vfxs shown after single matching
	public GameObject AlertBall;
	public GameObject ConfirmBall;
    public GameObject WrongBall;
	public int DestroySeconds = 1;
	[Header("Light Up Settings")]
	public GameObject[] Lights;
	//public float initEmissionScale = 1;
    public Material noLightMaterial;
	public int OnLitFrames = 200;
    public GameObject[] formerKeys;
    public GameObject targetDoor = null;
    [Header("Audio")]
    public AudioSource ActivatedClip;
    public AudioSource ActivateFailClip;


    private ArrayList onLightMaterial;
    private float[] lightRangeSteps, lightIntensitySteps;
	//private float lightEmissionScaleStep;
	private int curOnLitFrame;
	private KeyStatus status = KeyStatus.Inactive;
    private int curMatchNum = 0;
    private int targetMatchNum;
    private bool waitingSignal = true;
    private int signalArriveCount = 0;

	// Use this for initialization
	void Start()
	{
		InactiveBall.GetComponent<ParticleSystem>().Play(true);
        if (InactiveBall_2 != null)
            InactiveBall_2.GetComponent<ParticleSystem>().Play(true);
        ActiveBall.GetComponent<ParticleSystem>().Stop(true);
		lightRangeSteps = new float[Lights.Length];
		lightIntensitySteps = new float[Lights.Length];
        onLightMaterial = new ArrayList();
        //lightEmissionScaleStep = initEmissionScale / (float)OnLitFrames;
        //Debug.Log(lightEmissionScaleStep);
        //LightMaterial.SetFloat("_EMISSION", 0);
        int id = 0;
		foreach (GameObject lightObj in Lights)
		{
            Light light = lightObj.GetComponentInChildren<Light>();
            lightRangeSteps[id] = light.range / (float)OnLitFrames;
            light.range = 0;
			lightIntensitySteps[id] = light.intensity / (float)OnLitFrames;
            light.intensity = 0;
            light.enabled = false;
            
            Renderer[] srcRenderers = lightObj.GetComponentsInChildren<Renderer>();
            //Debug.Log(srcRenderers[0].materials[0].name);
            //Debug.Log(srcRenderers[0].materials[1].name);
            foreach (Renderer render in srcRenderers)
            {
                onLightMaterial.Add(render.materials.Clone());
                Material[] noLightMs = new Material[render.materials.Length];
                for (int i = 0; i < render.materials.Length; i++)
                    noLightMs[i] = noLightMaterial;
                render.materials = noLightMs;
            }
            //Debug.Log(srcRenderers[0].materials[0].name);
            //Debug.Log(srcRenderers[0].materials[1].name);
            id++;
		}
		curOnLitFrame = OnLitFrames;
    }

    void Update()
    {
        if (status == KeyStatus.OnShow && Input.GetKeyDown(KeyCode.Q))
        {
            TriggerShow();
        }

    }

	// Update is called once per frame
	void FixedUpdate()
	{
        if(status == KeyStatus.Inactive && InactiveBall_2 != null)
        {
            InactiveBall.transform.RotateAround(this.transform.position, Vector3.up, 90 * Time.deltaTime);
            InactiveBall_2.transform.RotateAround(this.transform.position, Vector3.up, 90 * Time.deltaTime);
        }
		if (curOnLitFrame < OnLitFrames)
		{
			curOnLitFrame++;
			int id = 0;
            //LightMaterial.SetFloat("_EMISSION", LightMaterial.GetFloat("_EMISSION") + lightEmissionScaleStep);
            foreach (GameObject lightObj in Lights)
            {
                Light light = lightObj.GetComponentInChildren<Light>();
                //light.GetComponent<Light>().enabled = false;
                light.range += lightRangeSteps[id];
                light.intensity += lightIntensitySteps[id];

                id++;
            }
		}
	}

	public string TriggerShow()
	{
        Debug.Log("Trigger normal key show");
		if (status == KeyStatus.Inactive)
		{
            this.transform.position = new Vector3(this.transform.position.x, GameObject.Find("LightBall").transform.position.y, this.transform.position.z);
			InactiveBall.SetActive(false);
            if(InactiveBall_2 != null)
                InactiveBall_2.SetActive(false);
            HintBall.SetActive(true);
            this.GetComponentInChildren<Pattern>().enabled = true;
            targetMatchNum = this.GetComponentInChildren<Pattern>().pattern.Length;
            //walk on stage and disable moving
            this.GetComponent<KeyCharacterController>().GotoKey();
            status = KeyStatus.OnShow;
            GameObject.Find("Character").GetComponent<Character>().NoConsumeDurability();
			//GetComponentInChildren<Pattern>().character = GetComponent<InputListener>().character;
			return "";
		}
		else if (status == KeyStatus.OnShow)
		{
            Debug.Log("haha");
            HintBall.SetActive(false);
            InactiveBall.SetActive(true);
            if (InactiveBall_2 != null)
                InactiveBall_2.SetActive(true);
            status = KeyStatus.Inactive;
            this.GetComponent<KeyCharacterController>().LeaveKey();
            GameObject.Find("Character").GetComponent<Character>().ConsumeDurability();
            //GameObject.Find("GameManager").GetComponent<GameManager>().StopMoveCtrl();
            //GetComponentInChildren<Pattern>().character = null;
            GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                "", DestroySeconds);
            return "Press E to dispaly.";
		}
		else
		{
			return "";
		}
	}

    private void activateWrong()
    {
        ActivateFailClip.Play();
        WrongBall.SetActive(true);
        HintBall.SetActive(false);
        GameObject alert = GameObject.Find("Alert");
        if (alert != null)
            alert.GetComponent<AlertController>().Alert();
        StartCoroutine(WaitAndInactive(WrongBall, DestroySeconds));
    }

    private void activateSuccess()
	{
        Debug.Log("called activated");
		this.GetComponentInChildren<Pattern>().enabled = false;
        ActivatedClip.Play();
        HintBall.SetActive(false);
        status = KeyStatus.Active;
        //ActiveBall.GetComponent<ParticleSystem>().Play(true);
        ActiveBall.SetActive(true);
        curOnLitFrame = 0;
        int id = 0;
        foreach (GameObject lightObj in Lights)
        {
            lightObj.GetComponentInChildren<Light>().enabled = true;
            Renderer[] curRenderers = lightObj.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < curRenderers.Length; i++)
            {
                Material[] srcRenderers = (Material[])onLightMaterial[id];
                curRenderers[i].materials = srcRenderers;
                //Debug.Log(srcRenderers[0].name);
                //Debug.Log(srcRenderers[1].name);
                id++;
            }
        }
        this.GetComponent<KeyCharacterController>().LeaveKey();
        GameObject.Find("Character").GetComponent<Character>().ConsumeDurability();
        this.GetComponent<InteractiveObj>().textContents[0] = "";
        //for(int i = 0; i < formerKeys.Length; i++)
        //    Showline(this.gameObject, formerKeys[i]);
        //if(targetDoor!=null)
         //   Showline(targetDoor, this.gameObject);
        //return "Activated";
    }

    private bool testActivateOrder()
    {
        if (formerKeys.Length == 1)
        {
            if (formerKeys[0].GetComponent<Key>().GetKeyStatus() != KeyStatus.Active)
            {
                //GameObject.Find("HintMessage").GetComponent<Text>().text = "This should not be activated now";
                activateWrong();
                return false;
            }
        }
        else if (formerKeys.Length == 2)
        {
            KeyStatus tmpStatus1 = formerKeys[0].GetComponent<Key>().GetKeyStatus();
            KeyStatus tmpStatus2 = formerKeys[1].GetComponent<Key>().GetKeyStatus();
            if (tmpStatus1 != KeyStatus.Active || tmpStatus2 != KeyStatus.Active)
            {
                //GameObject.Find("HintMessage").GetComponent<Text>().text = "This should not be activated now";
                activateWrong();
                return false;
            }
        }
        return true;
    }

    public void MatchSucceed()
	{
		GameObject tmp_ConfirmBall = Instantiate(ConfirmBall, this.transform);
		tmp_ConfirmBall.transform.position = HintBall.transform.position;
		tmp_ConfirmBall.transform.localScale = HintBall.transform.localScale;
		//HintBall.transform.localScale = new Vector3(0, 0, 0);
		//Debug.Log("haha");
        //Debug.Log(this.name);
        tmp_ConfirmBall.SetActive(true);
		StartCoroutine(WaitAndDestroy(tmp_ConfirmBall, DestroySeconds));
        curMatchNum ++;

        return;
	}

    public void MatchFail()
	{
		GameObject tmp_ConfirmBall = Instantiate(AlertBall, this.transform);
		tmp_ConfirmBall.transform.position = HintBall.transform.position;
		tmp_ConfirmBall.transform.localScale = HintBall.transform.localScale;
		//HintBall.transform.localScale = new Vector3(0, 0, 0);
		//Debug.Log("haha");
		tmp_ConfirmBall.SetActive(true);
		StartCoroutine(WaitAndDestroy(tmp_ConfirmBall, DestroySeconds, false));
        GameObject alert = GameObject.Find("Alert");
        if (alert != null)
            alert.GetComponent<AlertController>().Alert();
		return;
	}

    public void testAllMatch()
    {
        if (curMatchNum == targetMatchNum)
        {
            bool rightOrder = testActivateOrder();
            if (!rightOrder)
            {
                GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                    "This should not be activated now", DestroySeconds);
                activateWrong();
            }
            else
            {
                activateSuccess();
                if (targetDoor == null)
                {
                    GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                        "Activated. Keep on going.", DestroySeconds);
                    foreach (GameObject k in formerKeys)
                    {
                        Showline(this.gameObject, k);
                    }
                }
                else
                {
                    GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                        "Door has been unlocked", DestroySeconds * 2);
                    Showline(targetDoor, this.gameObject);
                    targetDoor.GetComponent<Door>().locked = false;
                    targetDoor.GetComponentInChildren<InteractiveObj>().hasDetected = true;
                }
            }
        }
        else
        {
            int showTime = HintBall.GetComponent<Pattern>().pitchLength;
            GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                    "You have to match all the pitches to activate it.", showTime);
            //StartCoroutine(WaitAndRestoreMessage(showTime));
        }
    }

	IEnumerator WaitAndDestroy(GameObject obj, int time, bool match = true)
	{
		yield return new WaitForSeconds(time);
		//HintBall.transform.localScale = obj.transform.localScale;
		if (!match)
			HintBall.transform.localScale = new Vector3(0, 0, 0);
		Destroy(obj);
	}

    IEnumerator WaitAndInactive(GameObject obj, int time)
    {
        Debug.Log("as");
        yield return new WaitForSeconds(time);
        //HintBall.transform.localScale = obj.transform.localScale;
        Debug.Log("as");
        obj.SetActive(false);
        InactiveBall.SetActive(true);
        if (InactiveBall_2 != null)
            InactiveBall_2.SetActive(true);
        status = KeyStatus.Inactive;
    }

    IEnumerator WaitAndRestoreMessage(int time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                   "Match the spheres with your voice.\n Press Q to stop", -1);

    }

    public void MatchReset()
    {
        curMatchNum = 0;
    }

    public KeyStatus GetKeyStatus()
    {
        return status;
    }

    private void Showline(GameObject target, GameObject source)
    {
        Waypoint t = target.GetComponent<Waypoint>();
        source.GetComponentInChildren<WaypointsHolder>().waypoints = new List<Waypoint> { t };
        source.GetComponentInChildren<WaypointsHolder>().enabled = true;
        source.GetComponentInChildren<WaypointMover>().enabled = true;
        source.transform.Find("Signal").gameObject.GetComponentInChildren<ParticleSystem>().Play(true);
        source.GetComponentInChildren<Signal>().enabled = true;
    }

    public void signalArrived(GameObject signal)
    {
        signalArriveCount++;
        //signal.SetActive(false);
        signal.GetComponentInChildren<ParticleSystem>().Stop(true);
        if (signalArriveCount == formerKeys.Length)
            activateSuccess();
    }
}
