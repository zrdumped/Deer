using UnityEngine;
using System.Collections;

[AddComponentMenu("Forge3D/Force Field/Force Field")]
public class Forcefield_Mobile : MonoBehaviour {

    // Force Field component cache variables
    private Material mat;
    private MeshFilter mesh;    

    // Number of controllable interpolators (impact points)
    private int interpolators = 6;

    // Unique shader propIDs (see http://docs.unity3d.com/ScriptReference/Shader.PropertyToID.html)
    // Used to modify shader interpolators by int id instead of string name
    private int[] shaderPropsID;    
    // Data containing xyz coordinate of impact and alpha in w stored in vector4 for each interpolator
    private Vector4[] shaderProps;

    // Current active interpolator
    int curProp = 0;
    // Timer used to advance trough interpolators
    float curTime = 0;
    
    // Force Field game object
    // Should be assigned trough the inspector
    // 
    // * IMPORTANT NOTE *
    // Note that collision events are only sent if one of the colliders also has a non-kinematic rigidbody attached.
    public GameObject shield;
    
    // Collision events flags
    [Header("Collision events:")]
    public bool CollisionEnter;

    // Speed at which interpolators will fade
    [Header("Shield settings:")]
    public float DecaySpeed = 2.0f;

    // Force Field reaction speed
    public float ReactSpeed = 0.1f;

    // Non-uniform scale correction
    public bool FixNonUniformScale;

	// INITIALIZATION
	void Start ()
    {
        // Cache required components
        mat = shield.GetComponent<Renderer>().material;
        mesh = shield.GetComponent<MeshFilter>();        

        // Generate unique IDs for optimised performance
        // since script has to access them each frame
        shaderPropsID = new int[interpolators];        
        for (int i = 0; i < interpolators; i++)        
            shaderPropsID[i] = Shader.PropertyToID("_Pos_" + i.ToString());

        // Initialize interpolators array
        shaderProps = new Vector4[interpolators];
	}

    // COLLISIONS EVENTS
    void OnCollisionEnter(Collision collisionInfo)
    {
        if(CollisionEnter)      
            foreach (ContactPoint contact in collisionInfo.contacts)        
                OnHit(contact.point);      
    }

    // MASK MANAGEMENT
    // Use this method to pass new impact points from any other script
    public void OnHit(Vector3 hitPoint, float hitAlpha = 1.0f)
    {        
        // Check reaction interval
        if (curTime >= ReactSpeed)
        {
            // Hit point coordinates are transforment into local space
            Vector4 newHitPoint = mesh.transform.InverseTransformPoint(hitPoint);

            // Clamp alpha value
            newHitPoint.w = Mathf.Clamp(hitAlpha, 0.0f, 1.0f);

            // Store new hit point data using current counter
            shaderProps[curProp] = newHitPoint;

            // Fix non-uniform scale
            if (FixNonUniformScale)
            {
                if (!Mathf.Approximately(transform.lossyScale.x, transform.lossyScale.y) || !Mathf.Approximately(transform.lossyScale.y, transform.lossyScale.z) || !Mathf.Approximately(transform.lossyScale.y, transform.lossyScale.z))
                {
                    shaderProps[curProp].x *= transform.lossyScale.x;
                    shaderProps[curProp].y *= transform.lossyScale.y;
                    shaderProps[curProp].z *= transform.lossyScale.z;
                }
            }

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
            if (shaderProps[i].w > 0f)
            {
                // Lerp alpha value for current interpolator
                shaderProps[i].w = Mathf.Lerp(shaderProps[i].w, -0.0001f, Time.deltaTime * DecaySpeed);
                shaderProps[i].w = Mathf.Clamp(shaderProps[i].w, 0f, 1f);
                // Assign new value to a shader variable
                mat.SetVector(shaderPropsID[i], shaderProps[i]);
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
