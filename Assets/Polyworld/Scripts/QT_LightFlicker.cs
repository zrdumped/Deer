using UnityEngine;
using System.Collections;


public class QT_LightFlicker : MonoBehaviour
{

   	public float minFlickerSpeed  = 0.01f;
    public float maxFlickerSpeed = 0.1f;
    public float minLightIntensity = 0.7f;
	public float maxLightIntensity =1;
    public bool ShiftLight = false;
    public float shiftAmount = 0.01f;

    void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
       Vector3 origin=this.transform.position;
       float i;
        while(true)
        {       
          i = Random.Range(minLightIntensity, maxLightIntensity);
          GetComponent<Light>().intensity = i;
          GetComponent<Light>().bounceIntensity = i/2;
          if (ShiftLight)          
              this.transform.position = new Vector3(origin.x + Random.Range(shiftAmount * -1, shiftAmount), origin.y + Random.Range(shiftAmount * -1, shiftAmount), origin.z + Random.Range(shiftAmount * -1, shiftAmount));

          yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));

        }
    }
}



