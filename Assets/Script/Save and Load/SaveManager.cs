using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



//2024.11.25
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    //[SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers = new List<ISaveManager>();
    private FileDataHandler dataHandler;

    private void InitializeDataHandler()
    {
        if (dataHandler == null)
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        }
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()//ɾ浵
    {
        InitializeDataHandler();
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
            //DontDestroyOnLoad(gameObject); // ʵڳлʱ


    }

    private void Start()
    {
        InitializeDataHandler();
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }


    public void NewGame()
    {
        gameData = new GameData();
    }


    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("ûҵ浵");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }


    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);

    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}

