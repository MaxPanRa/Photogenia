using UnityEngine;
using Cinemachine;

public class CharacterControl : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    public Transform orientation;
    public Transform characterCamera;
    public Transform playerObj;
    private CharacterController controller;
    private Rigidbody fprb;

    [Header("Movement")]
    public float rotationSpeed = 6f;
    public float moveSpeed = 100f;
    public float crouchSpeedMultiplier = 0.5f;
    public float runSpeedMultiplier = 1.5f;
    public float jumpSpeed = 3f;
    public LayerMask groundLayer;

    [Header("CameraControls")]
    public bool isCamera;
    public GameObject FPSCamera;

    [Header("DEBUG ONLY")]
    public bool isGrounded;

    private Vector3 movement;
    private float moveSpeedTmp;
    private float groundExtents;
    private float yVel;
    private CinemachineVirtualCamera fpsCM;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        fprb = GetComponent<Rigidbody>();
        groundExtents = GetComponent<Collider>().bounds.extents.y;
        fpsCM = FPSCamera.GetComponent<CinemachineVirtualCamera>();
        moveSpeedTmp = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Update()
    {
        //Declarando variables universales
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundExtents + 0.1f, groundLayer.value);
        moveSpeed = moveSpeedTmp;

        #region CROUCHING SECTION
        Crouching();
        #endregion

        #region RUNNING SECTION
        Running();
        #endregion

        #region MOVEMENT SECTION
        Movement(horizontalInput, verticalInput);
        #endregion

        #region JUMPING SECTION
        Jump();
        #endregion

        #region CAMERA SECTION
        CameraActivate();

        #endregion

    }

    private void Crouching()
    {
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            moveSpeed = moveSpeedTmp * crouchSpeedMultiplier;
        }
    }

    private void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            moveSpeed = moveSpeedTmp * runSpeedMultiplier;
        }
    }

    private void Movement(float horizontalInput, float verticalInput)
    {
        Vector3 viewDir = transform.position - new Vector3(characterCamera.position.x, transform.position.y, characterCamera.position.z);
        if (isCamera)
        {
            viewDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        }
        orientation.forward = viewDir.normalized;
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (inputDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            movement = Vector3.zero;
            yVel = fprb.velocity.y;
            Transform camTrans = Camera.main.transform;
             
            Debug.Log("VECTOR FRONTAL: "+ camTrans.forward);
            Debug.Log("VELOCIDAD FRONTAL: "+ camTrans.forward * moveSpeed * verticalInput);
            movement = camTrans.forward * moveSpeed * verticalInput;
            movement += camTrans.right * moveSpeed * horizontalInput;
            movement.y = yVel;
            fprb.velocity = movement;
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            fprb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void CameraActivate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isCamera = !isCamera;
        }
        if (isCamera)
        {
            fpsCM.Priority = 11;
            GetComponent<Character_Camera>().enabled = true;
            transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;

            if (isCamera)
            {
                //transform.forward = Vector3.Slerp(, Time.deltaTime * rotationSpeed * 20f);
            }
        }
        else
        {
            FPSCamera.GetComponent<CinemachineVirtualCamera>().Priority = 9;
            GetComponent<Character_Camera>().enabled = false;
        }
    }

    public void LoadData(GameData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
    }
    public void SaveData(GameData data)
    {
        data.position = transform.position;
        data.rotation = transform.rotation;
    }
}

