using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleBallistic;

[ExecuteInEditMode]
//[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class LaunchArc : MonoBehaviour
{
    /**
        Kinematic equations:
        1. s = ((u + v) * t) / 2
        2. v = u + a * t
        3. s = u * t + (a * t * t) / 2
        4. s = v * t - (a * t * t) / 2
        5. v * v = u * u + 2 * a * s
 
        v = final velocity
        u = initial velocity
        a = acceleration (gravity)
        s = displacement
        t = duration time
     */
    public Rigidbody rigidbody;
    public Transform target;
    public Transform shootingPosition;
    public Vector3 InitialVelocity;
    public LineRenderer lineRenderer;

    private float maximumHeightOfArc;
    public float gravity;
    public int pathResolution;
    public bool showDebugPath;

    public float releaseOffset;

    private bool isLaunching;
    private Vector3 savedPosition;

    private bool prepareToShoot;

    public GameObject teammateGameObject;
    private float teammateVelocity;

    private bool prepareToPass;

    private Vector3 oldPosition = new Vector3(0, 0, 0);
    private Vector3 newPosition = new Vector3(0, 0, 0);
    private Vector3 passTargetPosition;


    Vector3 charachterVelocity;
    Vector3 lastPosition;
    Vector3 distanceTraveled;

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float durationTime;

        public LaunchData(Vector3 velocity, float time)
        {
            this.initialVelocity = velocity;
            this.durationTime = time;
        }
    }


    LaunchData CalculateLaunchData()
    {

        float displacementY = target.position.y - this.transform.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - shootingPosition.transform.position.x, 0, target.position.z - shootingPosition.transform.position.z);
        float targetDistance = Vector3.Distance(this.transform.position, target.transform.position);

        float curveHeight = Mathf.Clamp(targetDistance, 0.5f, 3);

        float value = (displacementY - curveHeight) / gravity;
        float clampedCurve = Mathf.Clamp(value, 0, value);
        float time = Mathf.Sqrt(-2 * curveHeight / gravity) + Mathf.Sqrt(2 * clampedCurve);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * curveHeight);

        //multiply the XZ displacement by the release offset (which is set in the update loop)
        Vector3 velocityXZ = (displacementXZ * releaseOffset) / time;
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    LaunchData CalculatePass()
    {
        charachterVelocity = teammateGameObject.GetComponent<Movement>().ControllerVelocity;

        print(charachterVelocity);

        float displacementY = target.position.y - this.transform.position.y;
        float targetDistance = Vector3.Distance(this.transform.position, (target.transform.position + charachterVelocity));
        Vector3 displacementXZ = new Vector3((target.position.x + charachterVelocity.x) - shootingPosition.transform.position.x, 0, (target.position.z + charachterVelocity.z) - shootingPosition.transform.position.z);

        //float curveHeight = 1.0f;

        float curveHeight = Mathf.Clamp((target.position.magnitude + charachterVelocity.magnitude) * 0.1f, 0, 1);

        //float curveHeight = Mathf.Clamp((targetDistance * (charachterVelocity.magnitude * 0.1f)), displacementXZ.magnitude * 0.05f, 1);

        //print("targetDistance: " + targetDistance);

        //float curveHeight = Mathf.Clamp((targetDistance * (charachterVelocity.magnitude * 0.1f)), targetDistance / 0.1f, 1.0f);

        //print("charachterVelocity: " + charachterVelocity.magnitude);

        //float curveHeight = Mathf.Clamp((targetDistance * charachterVelocity.magnitude) * 0.1f, 0, 1.0f);

        //float curveHeight = Mathf.Clamp((targetDistance * charachterVelocity.magnitude), (targetDistance + charachterVelocity.magnitude) * 0.001f, 1.0f);

        //float curveHeight = Mathf.Clamp((targetDistance * charachterVelocity.magnitude), (targetDistance + charachterVelocity.magnitude) * 0.001f, 1.0f);

        //print("curveHeight: " + curveHeight);

        float value = (displacementY - curveHeight) / gravity;
        float clampedCurve = Mathf.Clamp(value, 0, value);

        float time = Mathf.Sqrt(-2 * curveHeight / gravity) + Mathf.Sqrt(2 * clampedCurve);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * curveHeight);
        Vector3 velocityXZ = displacementXZ  / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }


    /*
    LaunchData CalculatePassLaunchData()
    {

        float displacementY = target.position.y - this.transform.position.y;

        //Vector3 newPosition = teammateGameObject.transform.position;

        float teammateVelocity = teammateGameObject.GetComponent<CharacterController>().velocity.magnitude;

        Vector3 displacementXZ = new Vector3(target.position.x - shootingPosition.transform.position.x, 0, target.position.z - shootingPosition.transform.position.z);

        float targetDistance = Vector3.Distance(this.transform.position, target.transform.position);

        //float curveHeight = Mathf.Clamp(targetDistance, 0.5f, 3);

        float curveHeight = Mathf.Clamp(targetDistance, 0.1f, 0.3f);

        float value = (displacementY - curveHeight) / gravity;
        float clampedCurve = Mathf.Clamp(value, 0, value);
        float time = Mathf.Sqrt(-2 * curveHeight / gravity) + Mathf.Sqrt(2 * clampedCurve);


        float vectorDisplacement = (oldPosition - newPosition).magnitude;
        Vector3 direction = (oldPosition - newPosition).normalized;
        passTargetPosition = ((vectorDisplacement * time) * direction) * (targetDistance * teammateVelocity);

        //print("vectorDisplacement: " + vectorDisplacement);
        //print("time: " + Time.deltaTime);
        //print("direction: " + direction);
        //print(passTargetPosition);

        teammateGameObject.transform.GetChild(0).localPosition = new Vector3(0, 1.5f, Mathf.Abs(passTargetPosition.z));


        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * curveHeight);
        Vector3 velocityXZ = displacementXZ / time;

        //oldPosition = newPosition;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    } */

    void Launch()
    {

        this.isLaunching = true;
        shootingPosition.transform.LookAt(target.transform);
        Rigidbody clone = Instantiate(rigidbody, shootingPosition.transform.position, Quaternion.identity);
        clone.useGravity = true;

        if (prepareToPass)
        {
            clone.velocity = CalculatePass().initialVelocity;
        }

        else if (prepareToShoot)
        {
            clone.velocity = CalculateLaunchData().initialVelocity;
        }

        Destroy(clone.gameObject, 5);
    }

    /*
    Vector3 LookAtAngle(float _angle)
    {
        Vector3 axis = target.transform.position - transform.position;
        axis.y = 0;
        axis.Normalize();
        axis = Quaternion.AngleAxis(_angle, Vector3.Cross(axis, Vector3.up)) * axis;
        transform.forward = axis;

        return axis;
    }*/

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        if (float.IsNaN(launchData.initialVelocity.y) || float.IsInfinity(launchData.initialVelocity.y))
        {
            return;
        }
        //Vector3 originalPosition = rigidbody.position;

        Vector3 originalPosition = this.transform.position;

        Vector3[] positions = new Vector3[this.pathResolution + 1];
        for (int i = 0; i <= this.pathResolution; i++)
        {
            float simulationTime = (i / (float)this.pathResolution) * launchData.durationTime;
            Vector3 displacement = launchData.initialVelocity * simulationTime + (Vector3.up * gravity) * simulationTime * simulationTime / 2f;
            positions[i] = originalPosition + displacement;
        }
        this.lineRenderer.positionCount = positions.Length;
        this.lineRenderer.SetPositions(positions);

        this.InitialVelocity = launchData.initialVelocity;
    }

    void Awake()
    {
        //Rigidbody rigidbody = GetComponent<Rigidbody>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        //rigidbody.useGravity = false;
        this.isLaunching = false;
        Physics.gravity = Vector3.up * this.gravity;

        //lastPosition = teammateGameObject.transform.position;
    }

    void Update()
    {
        //TODO: Add character velocity to movement script (getter) then get the character velocity when aiming with raycast
        //distanceTraveled = target.transform.position - lastPosition;
        //lastPosition = target.transform.position;
        //charachterVelocity = distanceTraveled / Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.Space) || Input.GetButton("ShootBB"))
        {

            DrawPath();

            prepareToPass = false;
            prepareToShoot = true;

            releaseOffset += .002f;

            if (releaseOffset >= 1.1f)
            {
                prepareToShoot = false;

            }

        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("ShootBB"))
        {
            if (prepareToShoot)
            {
                Launch();
                releaseOffset = .95f;
            }

            if (!prepareToShoot)
            {
                releaseOffset = .95f;
            }
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pass"))
        {
            //this.target = teammatePassTarget;
            prepareToPass = true;
            prepareToShoot = false;
            Launch();
        }

        /*
        if (showDebugPath && !this.isLaunching)
        {
            DrawPath();
        }*/

        if (showDebugPath)
        {
            DrawPath();
        }
    }

    void OnValidate()
    {
        if (showDebugPath && !this.isLaunching)
        {
            DrawPath();
        }
    }
    /*
    private float TeammatePassTarget(GameObject teammate)
    {
        CharacterController teammateCharacterController = teammate.GetComponent<CharacterController>();
        float passTargetPosition = 0;

        print(teammateCharacterController.velocity.magnitude);

        if (teammateCharacterController.velocity.magnitude > 1)
        {
            passTargetPosition = passTargetPosition = teammateCharacterController.velocity.magnitude * 0.5f;
        }

        teammate.transform.GetChild(0).localPosition = new Vector3(0, 1.5f, passTargetPosition);

        return teammate.transform.GetChild(0).localPosition.z;
    }*/

    /*
    private float TeammatePassTarget(GameObject teammate)
    {
        Vector3 targetVelocity = teammate.GetComponent<CharacterController>().velocity;

        Vector3 targetPosition = teammate.transform.position + Math3d.ProjectVectorOnPlane(Vector3.up, targetVelocity);

        teammate.transform.GetChild(0).localPosition = targetPosition;

        return teammate.transform.GetChild(0).localPosition.z;
    }
    */

        /*
    private void DistancePerFrame(GameObject teammate)
    {
        Vector3 newPosition = teammate.transform.position;

        //print("new position: " + newPosition);
        //print("old position: " + oldPosition);

        float vectorDisplacement = (newPosition - oldPosition).magnitude;
        Vector3 direction = (newPosition - oldPosition).normalized;
        Vector3 passTargetPosition = ((vectorDisplacement * Time.deltaTime) * direction);

        //print("vectorDisplacement: " + vectorDisplacement);
        //print("time: " + Time.deltaTime);
        //print("direction: " + direction);
        //print(passTargetPosition);

        teammate.transform.GetChild(0).localPosition = passTargetPosition;

        oldPosition = newPosition;
    }*/
}