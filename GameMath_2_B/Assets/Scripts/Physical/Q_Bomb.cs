using UnityEngine;

public class Q_Bomb : MonoBehaviour
{
    private Rigidbody rb;
    private int bounceCount = 0;
    public float bounceBounciness = 0.8f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            bounceCount++;

            if (bounceCount >= 3)
            {
                Explode();
            }
            else
            {
                Vector3 inDirection = rb.linearVelocity;
                Vector3 normal = collision.contacts[0].normal;

                Vector3 outDirection = inDirection - 2 * Vector3.Dot(inDirection, normal) * normal;

                rb.linearVelocity = outDirection * bounceBounciness;
            }
        }
    }

    void Explode()
    {
        Debug.Log("Q 스킬");
        Destroy(gameObject);
    }
}