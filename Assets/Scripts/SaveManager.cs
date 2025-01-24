using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    PlayerInventory inventory;

    private const string saveFileName = "VertexMeshCommunicationSystem";

    private bool autoSave;

    /// <summary>
    /// Stores all values
    /// </summary>
    public SaveObject saveObject;

    #region Singleton
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        inventory = FindObjectOfType<PlayerInventory>();
        saveObject = new SaveObject();
    }

    #endregion

    private void Start()
    {
        Load();
        autoSave = true;
        StartCoroutine(AutoSave());
    }

    private void Save()
    {
        PlayerInventory.Instance.SaveInventoryState();
        saveObject.healthBarValue = PlayerHealth.Instance.curHealth;
        saveObject.hungerBarValue = HungerThirstManager.Instance.hungerValue;
        saveObject.thirstBarValue = HungerThirstManager.Instance.thirstValue;
        saveObject.sanityBarValue = PlayerSanity.Instance.currSanityLevel;
        saveObject.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        saveObject.keyList = new List<KeyEnum>(FinishDoorSingleton.Instance.GetKeyHash());
        saveObject.keyCount = FinishDoorSingleton.Instance.GetKeyCount();

        string json = JsonUtility.ToJson(saveObject);

        // Write to a file in the persistent data path
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(filePath, json);

        Debug.Log($"Game saved to {filePath}");
    }

    private void Load()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(filePath))
        {
            // Read the file and deserialize JSON
            string json = File.ReadAllText(filePath);
            saveObject = JsonUtility.FromJson<SaveObject>(json);

            // Apply the loaded data to PlayerInventory
            PlayerInventory.Instance.saveObject = saveObject;
            PlayerInventory.Instance.LoadInventoryState();
            PlayerHealth.Instance.curHealth = saveObject.healthBarValue;
            HungerThirstManager.Instance.hungerValue = saveObject.hungerBarValue;
            HungerThirstManager.Instance.thirstValue = saveObject.thirstBarValue;
            PlayerSanity.Instance.currSanityLevel = saveObject.sanityBarValue;
            GameObject.FindGameObjectWithTag("Player").transform.position = saveObject.playerPosition;
            FinishDoorSingleton.Instance.SetKeyHash(saveObject.keyList);
            FinishDoorSingleton.Instance.SetKeyCount(saveObject.keyCount);

            Debug.Log($"Game loaded from {filePath}");
        }
        else
        {
            // No save file found: Load default values
            Debug.LogWarning("Save file not found. Loading default values.");

            saveObject = new SaveObject
            {
                healthBarValue = 1f,
                hungerBarValue = 1f,
                thirstBarValue = 1f,
                sanityBarValue = 0f,
                playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position, // Keep current position
                keyList = new List<KeyEnum>(),
                keyCount = 0,

                // Default inventory values
                pistolCurrentAmmo = 6,
                pistolAmmo = 12, // Default starting ammo
                woodCount = 0,   // Example default wood count
                cookedMeatCount = 0,
                fleshCount = 0,
                ironCount = 0
            };

            // Apply default values to the game
            PlayerInventory.Instance.saveObject = saveObject;
            PlayerInventory.Instance.LoadInventoryState();
            PlayerHealth.Instance.curHealth = saveObject.healthBarValue;
            HungerThirstManager.Instance.hungerValue = saveObject.hungerBarValue;
            HungerThirstManager.Instance.thirstValue = saveObject.thirstBarValue;
            PlayerSanity.Instance.currSanityLevel = saveObject.sanityBarValue;
            FinishDoorSingleton.Instance.SetKeyHash(saveObject.keyList);
            FinishDoorSingleton.Instance.SetKeyCount(saveObject.keyCount);

            Debug.Log("Default values loaded.");
        }
    }

    private IEnumerator AutoSave()
    {
        while (autoSave)
        {
            yield return new WaitForSecondsRealtime(300f);
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}

/// <summary>
/// Is used to store all game nesseccary files to convert it to a json format lateron.
/// It has several variables to store each specific one from multiple scripts
/// </summary>
public class SaveObject
{
    public Vector3 playerPosition;

    //Player Values
    public float hungerBarValue;
    public float thirstBarValue;
    public float healthBarValue;
    public float sanityBarValue;

    //Game progress
    public List<KeyEnum> keyList; //stores all the keys, that have been found so far
    public int keyCount;

    //Inventory
    //Pistol
    public int pistolCurrentAmmo;
    public int pistolAmmo;
    //Wood
    public int woodCount;
    //Meat
    public int cookedMeatCount;
    //raw Meat
    public int fleshCount;
    //iron
    public int ironCount;
}
