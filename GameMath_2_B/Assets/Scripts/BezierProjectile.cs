using UnityEngine;
using System.Collections;

public class BezierProjectile : MonoBehaviour
{
    public void Launch(Vector3 start, Vector3 target, float duration)
    {
        StartCoroutine(MoveRoutine(start, target, duration));
    }

    IEnumerator MoveRoutine(Vector3 p0, Vector3 p3, float duration)
    {
        float elapsed = 0;

        // 징검다리 역할을 할 두 개의 조절점(Control Points) 생성
        // 적당히 랜덤한 위치를 주어 구체들이 사방으로 퍼졌다가 모이게 함
        Vector3 p1 = p0 + Random.insideUnitSphere * 5f + Vector3.up * 2f;
        Vector3 p2 = p3 + Random.insideUnitSphere * 5f + Vector3.up * 2f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // 3차 베지어 공식
            transform.position = CalculateCubicBezier(t, p0, p1, p2, p3);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = p3;
        // 충격 이펙트 재생 후 제거 로직 추가 가능
        Destroy(gameObject);
    }

    Vector3 CalculateCubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }
}
