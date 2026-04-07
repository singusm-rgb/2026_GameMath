using System.Linq;  
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StandardDeviation : MonoBehaviour
{
    public int sampleCount = 1000;
    public int randomMin = 0;
    public int randomMax = 100;
    void Start()
    {
        StandardDev();
    }

    void StandardDev()
    {
        int n = sampleCount;
        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(randomMin, randomMax);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);

        Debug.Log($"평균: {mean}, 표준편차: {stdDev}");
    }
}
