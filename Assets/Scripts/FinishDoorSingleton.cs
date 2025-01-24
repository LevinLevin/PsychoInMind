using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinishDoorSingleton : MonoBehaviour
{
    private int keyCount;

    private HashSet<KeyEnum> collectedKeys = new HashSet<KeyEnum>();

    #region Singleton
    public static FinishDoorSingleton Instance { get; private set; }

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
    }
    #endregion

    /// <summary>
    /// Adds a Key to the Collection if its no already in the collection
    /// </summary>
    /// <param name="collectedKey">needs the KeyEnum of the Key to compare it to the already collected keys</param>
    public void AddKey(KeyEnum collectedKey)
    {
        // Add the key to the HashSet. If it's already there, it won't be added again.
        if (collectedKeys.Add(collectedKey))
        {
            keyCount++;
            Debug.Log($"Key {collectedKey} collected! Total keys: {keyCount}");
        }
        else
        {
            // Key was already collected
            Debug.Log($"Key {collectedKey} is already collected.");
        }
    }

    /// <summary>
    /// Returns the HashSet of Keys
    /// </summary>
    /// <returns></returns>
    public HashSet<KeyEnum> GetKeyHash()
    {
        return collectedKeys;
    }

    /// <summary>
    /// Returns the number of keys as an integer
    /// </summary>
    /// <returns></returns>
    public int GetKeyCount()
    {
        return keyCount;
    }

    /// <summary>
    /// sets a list of KeyEnums and converts it to a HashSet
    /// </summary>
    /// <param name="keyList"></param>
    public void SetKeyHash(List<KeyEnum> keyList)
    {
        collectedKeys = new HashSet<KeyEnum>(keyList);
    }

    /// <summary>
    /// Sets the number of keys
    /// </summary>
    /// <param name="count">how many keys are there</param>
    public void SetKeyCount(int count)
    {
        keyCount = count;
    }
}

/// <summary>
/// Describes the Number of the Key that can be selected by the Player
/// </summary>
public enum KeyEnum
{
    FirstKey,
    SecondKey,
    ThirdKey
}
