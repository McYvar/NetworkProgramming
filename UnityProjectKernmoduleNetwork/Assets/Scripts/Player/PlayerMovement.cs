using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : BaseState, IGravity
{
    [SerializeField] protected SO_PlayerSheet playerSheet;
    [SerializeField] protected Transform cameraPivot;
    [SerializeField] protected Transform mainCamera;
    [SerializeField] protected Transform head;
    [SerializeField, Range(0, 0.2f)] private float slerpSpeed;
    [SerializeField] private LayerMask groundLayers;
    private Vector3 cameraTranslateVelocity;
    protected bool isSprinting;

    protected Rigidbody rb;
    protected bool isGrounded;

    protected Vector3 platformVelocity;
    protected static MovingPlatform platform;
    protected InputHandler inputHandler;

    private float smoothVelocity = 0;

    private Vector3 myGravityDirection;

    public override void Init()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        inputHandler = GetComponent<InputHandler>();
        myGravityDirection = Physics.gravity;
    }

    public override void OnEnter() 
    {
        inputHandler.pressOpenChatFirst += OpenChat;
        inputHandler.pressEscapeFirst += OpenPauseMenu;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnExit() 
    {
        inputHandler.pressOpenChatFirst -= OpenChat;
        inputHandler.pressEscapeFirst -= OpenPauseMenu;
    }

    public override void OnFixedUpdate() { }

    public override void OnUpdate()
    {
        GroundDetection();
        SprintDetect();
        if (rb.useGravity) FallTowardsGravity(myGravityDirection);
    }

    public override void OnLateUpdate()
    {
        CameraMovement(GlobalSettings.sensitivity);
    }

    protected void Movement(float speed)
    {
        // take current velocity, if standing with with y-axis up, then x & z should be 0 bij no input, however, this does not work when rotated
        // so we need the current velocity and need to zero the relative forward/backward and right/left.
        Vector3 rotatedInputVector = head.localRotation * new Vector3(inputHandler.horizontal, 0, inputHandler.vertical).normalized;
        Vector3 rotatedInputVelocity = new Vector3(
            rotatedInputVector.x * speed,
            0,
            rotatedInputVector.z * speed
            );

        rb.AddForce(transform.rotation * rotatedInputVelocity, ForceMode.VelocityChange);
    }

    private void SprintDetect()
    {
        if (inputHandler.vertical >= 0.1f && inputHandler.isPressedSprint) isSprinting = true;
        if (inputHandler.vertical < 0.1f) isSprinting = false;
    }

    protected void ReduceSpeed(float maxSpeed, float smoothTimeMoving, float smoothTimeNotMoving)
    {
        Vector3 rotatedVelocity = Quaternion.Inverse(transform.rotation) * rb.velocity;
        if (new Vector3(rotatedVelocity.x, 0, rotatedVelocity.z).magnitude > maxSpeed && (inputHandler.horizontal != 0 || inputHandler.vertical != 0))
        {
            float newMagnitude = Mathf.SmoothDamp(new Vector3(rotatedVelocity.x, 0, rotatedVelocity.z).magnitude, maxSpeed, ref smoothVelocity, smoothTimeMoving);
            Vector3 speedReductor = new Vector3(rotatedVelocity.x, 0, rotatedVelocity.z).normalized * newMagnitude + new Vector3(0, rotatedVelocity.y, 0);
            rb.velocity = transform.rotation * speedReductor;
        }
        else if (inputHandler.horizontal == 0 && inputHandler.vertical == 0)
        {
            float newMagnitude = Mathf.SmoothDamp(new Vector3(rotatedVelocity.x, 0, rotatedVelocity.z).magnitude, 0, ref smoothVelocity, smoothTimeNotMoving);
            Vector3 speedReductor = new Vector3(rotatedVelocity.x, 0, rotatedVelocity.z).normalized * newMagnitude + new Vector3(0, rotatedVelocity.y, 0);
            rb.velocity = transform.rotation * speedReductor;
        }
    }

    protected void Jump()
    {
        // Set upward velocity to 0
        Vector3 rotatedVelocity = Quaternion.Inverse(transform.rotation) * rb.velocity;
        rotatedVelocity.y = 0;
        rb.velocity = transform.rotation * rotatedVelocity;

        // add jump force
        rb.AddForce(transform.up * playerSheet.jumpStrength, ForceMode.VelocityChange);

        // switch to air state
        stateManager.SwitchState(typeof(InAir));
    }

    protected void GroundDetection()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        float sphereRadius = 0.9f;
        RaycastHit hit;
        if (Physics.SphereCast(ray,
            sphereRadius,
            out hit,
            transform.localScale.y - sphereRadius + 0.1f,
            groundLayers))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == 1 << 8) // ground layer
                {

                }
                else if (hit.collider.gameObject.layer == 1 << 9) // platform layer
                {
                    platform = hit.collider.GetComponent<MovingPlatform>();
                }
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            platform = null;
        }
    }

    protected void CameraMovement(float sensitivity)
    {
        cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, head.position, ref cameraTranslateVelocity, playerSheet.cameraTranslateSmoothTime);
        cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, transform.rotation, playerSheet.cameraRotateSmoothTime);

        Vector3 horizontalMouse = new Vector3(0, inputHandler.mouseDelta.x * sensitivity, 0);
        Vector3 verticalMouse = new Vector3(-inputHandler.mouseDelta.y * sensitivity, 0);

        head.localEulerAngles += horizontalMouse;
        float xRot = mainCamera.localEulerAngles.x + verticalMouse.x;
        if (xRot > 180) xRot -= 360;
        if (xRot < 180) xRot += 360;
        xRot = Mathf.Clamp(xRot, 270, 450);
        mainCamera.localEulerAngles = new Vector3(xRot,
                                                  mainCamera.localEulerAngles.y + horizontalMouse.y,
                                                  mainCamera.localEulerAngles.z);
    }

    public void OnEnterZone()
    {
    }

    public void SetGravity(Vector3 direction)
    {
        rb.useGravity = false;
        myGravityDirection = direction;
    }

    public void OnExitZone()
    {
        rb.useGravity = true;
        myGravityDirection = Physics.gravity;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    protected void FallTowardsGravity(Vector3 direction)
    {
        if (!rb.useGravity) rb.AddForce(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.FromToRotation(Vector3.down, direction),
            slerpSpeed);
    }

    private void OpenChat()
    {
        stateManager.SwitchState(typeof(InChat));
    }

    private void OpenPauseMenu()
    {
        stateManager.SwitchState(typeof(InPause));
    }
}
