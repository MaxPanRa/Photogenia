using UnityEngine;
using Cinemachine;

public class TPSController : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform mainCamera;
    public Transform groundCheck;
    public LayerMask playerMask;

    [Header("Velocidades")]
    public float speed = 5f;
    public float runSpeedMultiplier = 2.0f;
    public float crouchSpeedMultiplier = 0.5f;
    public float airControlMultiplier = 0.8f;
    public float fastAirControlMultiplier = 3f;
    public float turnSmoothTime = 0.08f;
    public float jumpSpeed = 5f;
    public float gravity = -9.81f;

    [Header("CameraControls")]
    public bool isCamera = false;
    public GameObject FPSCamera;
    public GameObject TPSCamera;
    public Vector3[] cameraOrbits = new[] { new Vector3(4, 8, 4), new Vector3(8, 12, 8), new Vector3(12, 16, 12) };

    [Header("Animation")]
    public Animator ModelAnimator;

    [Header("DEBUG?")]
    public bool isDebugging;

    [Header("Information")]
    public bool isGrounded;
    public bool isJumping;
    public bool isJumpingFast;
    public bool isWalking;
    public bool isRunning;
    public bool isCrouching;
    public bool isRunningControl;
    public bool isCrouchingControl;
    public float verticalSpeed = 0f;

    private float turnSmoothVelocity;
    private float currentSpeed;
    [SerializeField] private float verticalFall = 0.5f;
    [SerializeField] private float fallingLimit = 0.3f;
    [SerializeField] private float fallingSpeedLimit = -10f;
    [SerializeField] private float groundcheckRadius = 0.1f;
    private float fallingTimer = 0;
    private CapsuleCollider capsuleCollider;
    private Vector2 idleHeight;
    private Vector2 crouchHeight;
    private int currentOrbit = 1;
    private CinemachineFreeLook.Orbit[] orbits;

    private void Start()
    {
        currentSpeed = 0;
        capsuleCollider = GetComponent<CapsuleCollider>();
        idleHeight = new Vector2(capsuleCollider.height, controller.height);
        crouchHeight = new Vector2(0.9f, 0.9f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ActivateDeactivateCameraView(false);
        orbits = TPSCamera.GetComponent<CinemachineFreeLook>().m_Orbits;
    }
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isWalking = (horizontal > 0 || vertical > 0);
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        currentSpeed = speed;
        verticalSpeed = verticalSpeed < fallingSpeedLimit ? fallingSpeedLimit : verticalSpeed;

        isCrouchingControl = Input.GetKeyDown("joystick button 8") ? !isCrouchingControl : isCrouchingControl;
        isRunningControl = Input.GetAxisRaw("RunTrigger") != 0;

        #region FALLING
        if (isGrounded)
        {
            if (!Physics.CheckSphere(groundCheck.position, groundcheckRadius, ~playerMask)) {
                fallingTimer += Time.deltaTime;
                verticalSpeed += gravity * Time.deltaTime * verticalFall;
                controller.Move(verticalSpeed * Vector3.up * Time.deltaTime);
                if (fallingTimer > fallingLimit)
                {
                    verticalSpeed = gravity * 3 * Time.deltaTime;
                    isGrounded = false;
                    fallingTimer = 0;
                }
            }
            else
            {
                fallingTimer = 0;
            }
        }
        #endregion

        #region RUNNING
        isRunning = Input.GetKey(KeyCode.LeftShift) || (!isCrouchingControl && isRunningControl);
        if (isRunning && isGrounded)
        {
            if (isRunningControl)
            {
                currentSpeed *= Input.GetAxis("RunTrigger");
            }
            isWalking = false;
            currentSpeed = speed * runSpeedMultiplier;
        }
        #endregion

        #region CROUCHING
        isCrouching = Input.GetKey(KeyCode.LeftControl) || isCrouchingControl;
        if (isCrouching && isGrounded)
        {
            currentSpeed = speed * crouchSpeedMultiplier;
        }
        #endregion

        #region JUMP
        if (isJumping || isJumpingFast)
        {
            currentSpeed *= airControlMultiplier * (isJumpingFast ? fastAirControlMultiplier : 1f);
        }
        if (isGrounded)
        {
            isJumping = false;
            isJumpingFast = false;
            verticalSpeed = 0;
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && !isRunning)
            {
                isGrounded = false;
                isJumping = true;
                verticalSpeed = jumpSpeed;
                controller.Move(verticalSpeed * Vector3.up * Time.deltaTime);
            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isRunning) {
                isGrounded = false;
                isJumpingFast = true;
                verticalSpeed = jumpSpeed;
                controller.Move(verticalSpeed * Vector3.up * Time.deltaTime);
            }
        }
        else
        {
            verticalSpeed += gravity * Time.deltaTime;
            controller.Move(verticalSpeed * Vector3.up * Time.deltaTime);
        }

        #endregion

        #region MOVEMENT
        if (direction.magnitude != 0)
        {
            isWalking = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        #endregion

        #region CAMERA
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("DPadVertical") > 0)
        {
            // CONTROL DE CAMARA EN MENUMANAGER.CS
            //isCamera = !isCamera;
            //ActivateDeactivateCameraView();
        }
        if (isCamera)
        {
            transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            GetComponent<Character_Camera>().enabled = true;
        }
        else
        {
            GetComponent<Character_Camera>().enabled = false;
        }
        #endregion

        #region PLAYER CAMERA ZOOM
        float mouseScroll = Input.mouseScrollDelta.y;
        bool RSB = Input.GetKeyDown("joystick button 9");
        if (RSB)
        {
            if (currentOrbit >= cameraOrbits.Length-1) {
                currentOrbit = 0;
            }
            else
            {
                currentOrbit++;
            }
            Debug.Log("RSB:" + RSB + " --- currentOrbit: " + currentOrbit+" ---Orbits:"+ cameraOrbits.Length);
            orbits[0].m_Radius = cameraOrbits[currentOrbit].x;
            orbits[1].m_Radius = cameraOrbits[currentOrbit].y;
            orbits[2].m_Radius = cameraOrbits[currentOrbit].z;
            TPSCamera.GetComponent<CinemachineFreeLook>().m_Orbits = orbits;
        }
        if (mouseScroll != 0)
        {
            Debug.Log("MouseScroll:" + mouseScroll + " --- currentOrbit: " + currentOrbit);
            if (mouseScroll > 0)
            {
                if (currentOrbit <= 0)
                {
                    currentOrbit = cameraOrbits.Length-1;
                }
                else
                {
                    currentOrbit--;
                }
            }
            if (mouseScroll < 0)
            {
                if (currentOrbit >= cameraOrbits.Length-1)
                {
                    currentOrbit = 0;
                }
                else
                {
                    currentOrbit++;
                }
            }
            Debug.Log(orbits.ToString());
            orbits[0].m_Radius = cameraOrbits[currentOrbit].x;
            orbits[1].m_Radius = cameraOrbits[currentOrbit].y;
            orbits[2].m_Radius = cameraOrbits[currentOrbit].z;
            TPSCamera.GetComponent<CinemachineFreeLook>().m_Orbits = orbits;
        }
        #endregion

        #region ANIMATIONS
        ModelAnimator.SetBool("OnGround", isGrounded);
        ModelAnimator.SetBool("Crouch", isCrouching);
        ModelAnimator.SetFloat("Forward", vertical * (isRunning ? 2 : 1));
        ModelAnimator.SetFloat("Turn", horizontal * (isRunning ? 2 : 1));
        ModelAnimator.SetFloat("VerticalSpeed", verticalSpeed);
        ModelAnimator.SetFloat("IsFastJumping", isJumpingFast ? 1 : 0);
        #endregion
        
    }


    void OnTriggerStay(Collider collisionInfo)
    {
        if (!collisionInfo.CompareTag("Player") && !collisionInfo.CompareTag("WaterFilter") && !collisionInfo.CompareTag("MainCamera"))
        {
            if(isDebugging)
                Debug.DrawRay(collisionInfo.ClosestPoint(transform.position), Vector3.up * 20, Color.red);
            isGrounded = true;
            verticalSpeed = 0;
        }
    }

    public void ActivateDeactivateCameraView(bool isCamera)
    { 
        TPSCamera.SetActive(!isCamera);
        FPSCamera.SetActive(isCamera);
    }

    public void TransportCharacter(Vector3 position, Quaternion rotation)
    {
        this.transform.position = position;
        this.transform.rotation= rotation;
    }
}
