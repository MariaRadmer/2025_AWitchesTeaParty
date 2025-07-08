using UnityEngine;

public class Projectile : MonoBehaviour, IObjectPoolObject
{
    public float upForce = 5f;
    public float sideForce = 2f;

    public void UpdatePoolObject()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce, upForce);

        Vector3 force = new Vector3(xForce, yForce);
        GetComponent<Rigidbody2D>().linearVelocity = force;
    }


}
