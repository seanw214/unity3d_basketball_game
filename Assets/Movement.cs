using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 ControllerVelocity { get; private set; }

    public FloatingJoystick joystick;
    [SerializeField]
    private float joystickSpeed;
    [SerializeField]
    private int rotateSpeed;
    private CharacterController controller;
    private Vector3 movementDirection;
    private Vector3 lastPosition;
    private Vector3 distanceTraveled;

    public bool useGamepadInput;
    public bool useTouchControls;
    public bool useWASD;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        distanceTraveled = this.transform.position - lastPosition;
        lastPosition = this.transform.position;
        ControllerVelocity = distanceTraveled / Time.fixedDeltaTime;

        if (useGamepadInput)
        {
            movementDirection.z = -Input.GetAxis("Horizontal") / joystickSpeed;
            movementDirection.x = Input.GetAxis("Vertical") / joystickSpeed;
        }

        if (useWASD)
        {
            if (Input.GetButton("a"))
            {
                movementDirection.z = 1 / joystickSpeed;
            }
            else if (Input.GetButton("d"))
            {
                movementDirection.z = -1 / joystickSpeed;
            }
            else if(Input.GetButton("w"))
            {
                movementDirection.x = 1 / joystickSpeed;
            }
            else if(Input.GetButton("s"))
            {
                movementDirection.x = -1 / joystickSpeed;
            }
            else if (Input.GetButton("d") && Input.GetButton("w"))
            {
                movementDirection.z = -0.5f / joystickSpeed;
                movementDirection.z = 0.5f / joystickSpeed;
            }
            else if (Input.GetButton("w") && Input.GetButton("a"))
            {
                movementDirection.x = 0.5f / joystickSpeed;
                movementDirection.z = 0.5f / joystickSpeed;
            }
            else if (Input.GetButton("s") && Input.GetButton("d"))
            {
                movementDirection.x = -0.5f / joystickSpeed;
                movementDirection.z = -0.5f / joystickSpeed;
            }
            else if (Input.GetButton("s") && Input.GetButton("a"))
            {
                movementDirection.x = -0.5f / joystickSpeed;
                movementDirection.z = 0.5f / joystickSpeed;
            }
            //no keys are pressed
            else
            {
                movementDirection = Vector3.zero;
            }
        }

        if (useTouchControls)
        {
            movementDirection.z = -joystick.Horizontal / joystickSpeed;
            movementDirection.x = joystick.Vertical / joystickSpeed;
        }

        //if movementDirection is not at zero, character controller is moving
        if (movementDirection != Vector3.zero)
        {
            //move character controller in axis direction
            controller.Move(movementDirection);

            //rotate character controller over time
            Quaternion direction = Quaternion.LookRotation(movementDirection);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, direction, rotateSpeed * Time.deltaTime);
        }
    }
}
