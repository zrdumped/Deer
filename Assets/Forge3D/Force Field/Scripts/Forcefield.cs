using UnityEngine;
using System.Collections;

[AddComponentMenu("Forge3D/Force Field/Force Field")]
public class Forcefield : MonoBehaviour {

    private new Transform transform;

    // Force Field component cache variables
    private Material mat;
    private MeshFilter mesh;    

    // Number of controllable interpolators (impact points)
    private int interpolators = 24;

    // Unique shader propIDs (see http://docs.unity3d.com/ScriptReference/Shader.PropertyToID.html)
    // Used to access shader variables with int id instead of string name
    private int[] shaderPosID, shaderPowID;    

    // Data containing xyz coordinate of impact and alpha in w for each interpolator    
    private Vector4[] shaderPos;   

    // Current active interpolator
    int curProp = 0;
    // Timer used to advance trough interpolators
    float curTime = 0;
    
    // Force Field game object
    // Should be assigned trough the inspector
    // 
    // * IMPORTANT NOTE *
    // Note that collision events are only sent if one of the colliders also has a non-kinematic rigidbody attached.
    [Header("Force Field object:")]
    public GameObject Field;
    
    // Collision events flags
    [Header("Collision events:")]
    public bool CollisionEnter;
    public bool CollisionStay;
    public bool CollisionExit;

    // Speed at which interpolators will fade
    [Header("Shield settings:")]
    public float DecaySpeed = 2.0f;

    // Force Field reaction speed
    public float ReactSpeed = 0.1f;

    // Non-uniform scale correction
    public bool FixNonUniformScale;

    void Awake() { transform = GetComponent<Transform>(); }

	// INITIALIZATION
	void Start ()
    {
        // Cache required components
        mat = Field.GetComponent<Renderer>().material;
        mesh = Field.GetComponent<MeshFilter>();        

        // Generate unique IDs for optimised performance
        // since script has to access them each frame
        shaderPosID = new int[interpolators];
        shaderPowID = new int[interpolators];

        for (int i = 0; i < interpolators; i++)
        {
            shaderPosID[i] = Shader.PropertyToID("_Pos_" + i.ToString());
            shaderPowID[i] = Shader.PropertyToID("_Pow_" + i.ToString());
        }

        // Initialize data array
        shaderPos = new Vector4[interpolators];      
	}

    // COLLISIONS EVENTS
    void OnCollisionEnter(Collision collisionInfo)
    {
        if(CollisionEnter)      
            foreach (ContactPoint contact in collisionInfo.contacts)        
                OnHit(contact.point);      
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (CollisionStay)     
            foreach (ContactPoint contact in collisionInfo.contacts)       
                OnHit(contact.point);       
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (CollisionExit)
            foreach (ContactPoint contact in collisionInfo.contacts)
                OnHit(contact.point);            
    }

    // MASK MANAGEMENT
    // 
    /// <summary>
    /// Use this method to send impact coordinates from any other script
    /// </summary>
    /// <param name="hitPoint">Worldspace hit point coordinates</param>
    /// <param name="hitPower">Hit strength</param>
    /// <param name="hitAlpha">hit alpha</param>
    public void OnHit(Vector3 hitPoint, float hitPower = 0.0f, float hitAlpha = 1.0f)
    {  
        // Check reaction interval
        if (curTime >= ReactSpeed)
        {
            // Hit point coordinates are transformed into local space
            Vector4 newHitPoint = mesh.transform.InverseTransformPoint(hitPoint);

            // Clamp alpha
            newHitPoint.w = Mathf.Clamp(hitAlpha, 0.0f, 1.0f); 

            // Store new hit point data using current counter
            shaderPos[curProp] = newHitPoint;

            // Fix non-uniform scale
            if (FixNonUniformScale)
            {
                if(!Mathf.Approximately(transform.lossyScale.x, transform.lossyScale.y) || !Mathf.Approximately(transform.lossyScale.y, transform.lossyScale.z) || !Mathf.Approximately(transform.lossyScale.y, transform.lossyScale.z))
                {
                    shaderPos[curProp].x *= transform.lossyScale.x;
                    shaderPos[curProp].y *= transform.lossyScale.y;
                    shaderPos[curProp].z *= transform.lossyScale.z;
                }
            }

            // Send hitPower into a shader
            mat.SetFloat(shaderPowID[curProp], hitPower);

            // Reset timer and advance counter
            curTime = 0.0f;
            curProp++;
            if (curProp == interpolators) curProp = 0;
        }
    }

    // Called each frame to pass values into a shader
    void FadeMask()
    {
        for (int i = 0; i < interpolators; i++)
        {
            if (shaderPos[i].w > 0f)
            {
                // Lerp alpha value for current interpolator
                shaderPos[i].w = Mathf.Lerp(shaderPos[i].w, -0.0001f, Time.deltaTime * DecaySpeed);
                shaderPos[i].w = Mathf.Clamp(shaderPos[i].w, 0f, 1f);
                // Assign new value to a shader variable
                mat.SetVector(shaderPosID[i], shaderPos[i]);
            }
        }
    }
        
	// UPDATE
	void Update ()
    {   
        // Advance response timer
        curTime += Time.deltaTime;
        // Update shader each frame
        FadeMask();
	}
}
