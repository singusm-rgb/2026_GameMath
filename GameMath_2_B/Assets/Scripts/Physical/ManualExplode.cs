using UnityEngine;

public class ManualExplode : MonoBehaviour
{
    public float delay = 1.5f;
    public float radius = 5f;
    public float force = 300f;
    public float upwardModifier = 1f;

    private void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPos, radius, upwardModifier);
            }
        }
        Destroy(gameObject);
    }
}
