using UnityEngine;

public class Explode : MonoBehaviour
{
    public float force = 300f;

    public float radius = 5f;

    private void Start()
    {
        Invoke("RunExplode", 2f);
    }

    void RunExplode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] hitcolliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in hitcolliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPos, radius);
            }
        }
        Destroy(gameObject);
    }
}
