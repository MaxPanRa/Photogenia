using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] private float autoSaveTimeSeconds = 360f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private Coroutine autoSaveCoroutine;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loading Scene");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Saving Scene");
        SaveGame();
    }

    private void Start()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("GUARDANDO...");
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("CARGANDO...");
            LoadGame();
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Cargar datos guardados de un archivo utilizando data handler
        this.gameData = dataHandler.Load();

        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        
        //Si no encuentra datos, inicializar un nuevo juego
        if (this.gameData == null) {
            Debug.Log("No data was found. Initializing data to defaults ");
            NewGame();
        }

        // Envia los datos a todos los scripts que lo requieren
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    
    public void SaveGame()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Pasa los datos de los otros scripts para actualizar la informacion
        if (gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // Guarda los datos a un archivo utilziando data handler
        dataHandler.Save(gameData);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }



    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public IEnumerator AutoSave()
    {
        while (false)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("AutoSave");
        }
    }
}
