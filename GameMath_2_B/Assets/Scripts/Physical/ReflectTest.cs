using UnityEngine;

public class ReflectTest : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, 3f, 0f);
    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal.normalized;

        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflect = velocity - 2f * dot * normal;

        velocity = reflect;
    }
}
