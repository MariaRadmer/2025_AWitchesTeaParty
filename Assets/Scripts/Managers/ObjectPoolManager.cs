using System.Collections.Generic;
using UnityEngine;


public enum PoolObjectTag
{
    Projectile
}


[System.Serializable]
public class Pool { 
    public PoolObjectTag tag;
    public GameObject prefab;
    public int size; 
}

public class ObjectPoolManager : MonoBehaviour
{

    public static ObjectPoolManager Instance { get; private set; }
    public List<Pool> pools;
    public Dictionary<PoolObjectTag, Queue<GameObject>> poolDictionary; 

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<PoolObjectTag, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, queue);
        }
    }

    public GameObject SpawnFromPool (PoolObjectTag tag, Vector2 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Tag does not exsist " + tag);
            return null;
        }          

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        IObjectPoolObject poolObject = objToSpawn.GetComponent<IObjectPoolObject>();

        if (poolObject != null)
        {
            poolObject.UpdatePoolObject();
        }


        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
