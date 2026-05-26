using UnityEngine;

public class W_Bomb : MonoBehaviour
{
    public float explosionRadius = 5f; // 폭발 범위
    public float knockbackPower = 15f; // 넉백 힘
    public float explosionDelay = 2f;  // 폭발까지 걸리는 시간

    void Start()
    {
        // 생성 후 일정 시간 뒤에 Explode 함수 실행
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        Debug.Log("W 폭탄 펑!");

        // 폭발 반경 내의 모든 콜라이더 검출 (플레이어 포함)
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();

            if (hitRb != null)
            {
                // AddExplosionForce를 사용하지 않고 직접 밀어내는 힘 계산
                // 방향: 폭탄 위치에서 대상의 위치를 향하는 방향
                Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;

                // 폭발의 입체감을 위해 약간 위쪽으로 띄워줌
                knockbackDir.y += 0.5f;
                knockbackDir.Normalize();

                // 순간적인 힘(Impulse)으로 대상 밀어내기
                hitRb.AddForce(knockbackDir * knockbackPower, ForceMode.Impulse);
            }
        }

        // 폭탄 오브젝트 제거
        Destroy(gameObject);
    }
}