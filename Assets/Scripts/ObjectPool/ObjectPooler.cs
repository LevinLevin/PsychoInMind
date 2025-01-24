using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lets you create pools of object that can setActive the item when the player needs it withou using instantiate every time
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ItemEnum tag;
        public GameObject prefab;
        public int size;
    }

    #region Singelton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag.ToString(), objectPool);
        }
    }

    public GameObject SpawnFromPool(ItemEnum tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag.ToString()))
            return null;

        GameObject objectToSpawn = poolDictionary[tag.ToString()].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag.ToString()].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}