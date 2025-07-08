using UnityEngine;

public class ObjectPoolSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private ObjectPoolManager poolManager;

    private void Start()
    {
        poolManager = ObjectPoolManager.Instance;
    }

    void FixedUpdate()
    {
        poolManager.SpawnFromPool(PoolObjectTag.Projectile,transform.position,transform.rotation);
    }
}
