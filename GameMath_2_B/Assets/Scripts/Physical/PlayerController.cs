using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject qBombPrefab;
    public GameObject wBombPrefab;

    public float qLaunchPower = 15f;
    public float wSpawnDistance = 4f;

    void Update()
    {
        // [Q 스킬] 플레이어 위치에서 발사
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 플레이어의 약간 앞, 위쪽에서 생성
            Vector3 spawnPos = transform.position + transform.forward + Vector3.up;
            GameObject qBomb = Instantiate(qBombPrefab, spawnPos, transform.rotation);

            // 플레이어가 보고 있는 방향으로 힘을 가함
            Rigidbody rb = qBomb.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * qLaunchPower, ForceMode.VelocityChange);
        }

        // [W 스킬] 플레이어 앞 일정 거리에 생성
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 플레이어가 바라보는 방향 앞(wSpawnDistance) 위치 계산
            Vector3 spawnPos = transform.position + (transform.forward * wSpawnDistance);
            spawnPos.y = 0.5f; // 바닥에 파묻히지 않도록 높이 조정

            Instantiate(wBombPrefab, spawnPos, Quaternion.identity);
        }
    }
}