using UnityEngine;

public class GenerateGaussian : MonoBehaviour
{
    public float mean = 50.0f;
    public float stddev = 10.0f;

    public void Test()
    {
        Debug.Log(Ganerate(mean, stddev));
    }
    float Ganerate(float mean, float stddev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stddev * randStdNormal;
    }
}
