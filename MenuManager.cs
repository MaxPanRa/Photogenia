using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("HUD Bools")]
    public bool isNormal = true;
    public bool isCamera = false;
    private bool showCameraHUD=false;
    public bool isCameraMenu = false;
    public bool isCameraMenuDetail = false;
    public bool isPaused = false;
    public bool isInventory = false;
    public bool isPC = true;
    public bool isXbox = false;
    public bool hasResettedTimeScale = true;
    [Header("HUD Screens")]
    public GameObject NormalHUD;
    public GameObject CameraHUD;
    public GameObject CameraMenuHUD;
    public GameObject CameraMenuDetailHUD;
    public GameObject PauseHUD;
    public GameObject InventoryHUD;
    public GameObject GalleryContent;

    [Header("Referencias")]
    public TPSController TPSController;
    public GameObject ANIM_CAMARA;
    public GameObject playerEnergy;
    public GameObject cameraBattery;
    public GameObject photoTypes;
        private TextMeshPro prevType;
        private TextMeshPro nextType;
        private TextMeshPro currentType;
    public TextMeshProUGUI availablePhotos;

    #if UNITY_EDITOR
        KeyCode esc = KeyCode.X;
    #else
    KeyCode esc = KeyCode.Escape;
    #endif
    private void Start()
    {
        instance = this;
        prevType = photoTypes.transform.Find("PrevType").GetComponent<TextMeshPro>();
        nextType = photoTypes.transform.Find("NextType").GetComponent<TextMeshPro>();
        currentType = photoTypes.transform.Find("CurrentType").GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        /* BOTONES PARA EL MENU
         * Q - Abrir Camara
         * ESC - Pausa
         * CON CAMARA ABIERTA
         * G - Abrir Galeria
         * Q - Cerrar Camara
         * ESC - Pausa
         * CON GALERIA ABIERTA (Desbloquear cursor)
         * ESC - SALIR a Camara Abierta
         * CLICK EN IMAGEN - Abrir Detalle Camara
         * CON GALERIA DETALLE ABIERTO
         * ESC - SALIR a Galeria Abierta
         * */
        Cursor.lockState = CursorLockMode.Locked;

        if (!GameManager.instance.isMovementTutorial)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("DPadVertical") > 0)
            {
                Debug.Log("Click_1");
                this.isCamera = !isCamera;
                this.isNormal = !isCamera;
                if (this.isCamera)
                {
                    ANIM_CAMARA.SetActive(true);
                    TPSController.ActivateDeactivateCameraView(true);
                    ActivateCamera();
                    Invoke("DeactivateANIM_CAM", 3f);
                }
                else
                {
                    showCameraHUD = false;
                    TPSController.isCamera = false;
                    TPSController.ActivateDeactivateCameraView(false);
                    ANIM_CAMARA.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                if (this.isCamera)
                {
                    GameManager.instance.GetPictures();
                    this.isCamera = false;
                    TPSController.isCamera = false;
                    isCameraMenu = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isCameraMenu)
                {
                    isCameraMenu = false;
                    showCameraHUD = true;
                    isCamera = true;
                    TPSController.isCamera = true;
                }else
                if (isCameraMenuDetail)
                {
                    TPSController.isCamera = false;
                    GameManager.instance.GetPictures();
                    isCameraMenuDetail = false;
                    isCameraMenu = true;
                }else
                if (isPaused)
                {
                    ExitPauseMenu();
                }
                else
                {
                    TPSController.isCamera = false;
                    this.isCamera = false;
                    this.isNormal = false;
                    isPaused = true;
                }
            }
        } 

        ResetHUD();
        if (showCameraHUD && isCamera)
        {
            TPSController.isCamera = true;
            Cursor.visible = false;
            CameraHUD.SetActive(true);
        }
        if(isCameraMenu)
        {
            TPSController.isCamera = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CameraMenuHUD.SetActive(true);
        }
        if (isCameraMenuDetail)
        {
            TPSController.isCamera = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CameraMenuDetailHUD.SetActive(true);
        }
        if (isInventory)
        {
            TPSController.isCamera = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InventoryHUD.SetActive(true);
        }
        if (isPaused)
        {
            TPSController.isCamera = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PauseHUD.SetActive(true);
        }
        if(isNormal)
        {
            TPSController.isCamera = false;
            Cursor.visible = false;
            NormalHUD.SetActive(true);
        }

}
    public void ExitCamMenu()
    {
        Time.timeScale = 1f;
        isCameraMenu = false;
        isCamera = true;
        TPSController.isCamera = true;
    }
    public void ReturnCamMenu()
    {
        Time.timeScale = 0.0000000000001f;
        isCameraMenuDetail = false;
        isCameraMenu = true;
    }
    public void ExitInventoryMenu()
    {
        Time.timeScale = 1f;
        isInventory = false;
        isNormal = true;
    }
    public void ExitPauseMenu()
    {
        ResetHUD();
        this.isCamera = false;
        this.isNormal = true;
        showCameraHUD = false;
        TPSController.isCamera = false;
        TPSController.ActivateDeactivateCameraView(false);
        ANIM_CAMARA.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void ResetHUD()
    {
        NormalHUD.SetActive(false);
        CameraHUD.SetActive(false);
        CameraMenuHUD.SetActive(false);
        CameraMenuDetailHUD.SetActive(false);
        PauseHUD.SetActive(false);
        InventoryHUD.SetActive(false);
}
    public void ActivateCamera()
    {
        Invoke("CamOnDelay", 1.5f);
    }
    private void DeactivateANIM_CAM()
    {
        ANIM_CAMARA.SetActive(false);
        TPSController.isCamera = isCamera;
    }
    private void CamOnDelay()
    {
        showCameraHUD = true;
    }
    public void ActivateCameraMenu()
    {
        ResetHUD();
        isCamera = !isCamera;
    }
    public void ActivateCameraMenuDetail()
    {
        ResetHUD();
        isCamera = true;
    }
    public void ActivateInventory()
    {
        ResetHUD();
        isNormal = true;
        isCamera = true;
    }

    public void ShowImages(Sprite[] sprites)
    {
        foreach (Image photo in GalleryContent.transform.GetComponentsInChildren<Image>(true))
        {
            photo.gameObject.SetActive(false);
        }
            int len = sprites.Length;
        int counter = 0;
        foreach (Image photo in GalleryContent.transform.GetComponentsInChildren<Image>(true))
        {
            if (len == counter)
            {
                return;
            }
            photo.sprite = sprites[counter];
            photo.gameObject.SetActive(true);
            counter++;
        }
    }
}
