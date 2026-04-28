using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform point1; // 첫 번째 지점을 위한 빈 게임 오브젝트 할당
    public Transform point2; // 두 번째 지점을 위한 빈 게임 오브젝트 할당
    public float moveSpeed = 2f; // 이동 속도

    private Transform targetPoint;

    void Start()
    {
        targetPoint = point1; // point1을 향해 이동 시작
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == point1)
            {
                targetPoint = point2;
            }
            else
            {
                targetPoint = point1;
            }
        }
    }
}

