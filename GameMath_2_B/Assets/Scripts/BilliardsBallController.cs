using UnityEngine;
using System.Collections.Generic;

public class BilliardsBallController : MonoBehaviour
{
    public BilliardsGameManager.Turn ballOwner; // 이 공의 주인 (Player1 또는 Player2)
    public string opponentTag;                 // 상대방 공의 Tag
    public string targetTag = "TargetBall";     // 타겟 공의 Tag

    [Header("Physics Settings")]
    public float baseForce = 5f;
    public float maxForce = 25f;
    public float chargeSpeed = 15f; // 초당 충전되는 힘의 세기

    [HideInInspector] public bool hitOpponent = false;
    [HideInInspector] public int hitTargetCount = 0;
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    private Rigidbody rb;
    private Vector3 pendingForce;
    private bool shouldApplyForce = false;
    private float currentPower = 0f;
    private bool isCharging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetTurnFlags();
    }

    private void Update()
    {
        // 1. 현재 자신의 턴이 아니거나 공이 움직이는 중이면 추가 입력 완전 차단
        if (BilliardsGameManager.Instance.currentTurn != ballOwner) return;
        if (!BilliardsGameManager.Instance.CanPlayerClick()) return;

        // 2. 마우스 클릭 시 (차지 시작)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isCharging = true;
                    currentPower = baseForce;
                }
            }
        }

        // 3. 마우스를 누르고 있는 동안 힘 게이지 상승 (+@ 보너스 힘 조절 기능)
        if (isCharging && Input.GetMouseButton(0))
        {
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Min(currentPower, maxForce);
        }

        // 4. 마우스를 뗄 때 계산된 방향과 힘으로 발사 준비
        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 힘의 방향 = 공의 중심 - 클릭 지점 (슬라이드 18강 공식)
                Vector3 clickPoint = hit.point;
                clickPoint.y = transform.position.y; // Y축 오차를 없애 물리 버그 방지

                Vector3 forceDirection = (transform.position - clickPoint).normalized;
                pendingForce = forceDirection * currentPower;
                shouldApplyForce = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // 슬라이드 12강 지침: 물리 연산(AddForce)은 반드시 FixedUpdate에서 수행
        if (shouldApplyForce)
        {
            // 슬라이드 9강 지침: 순간적으로 강한 충격을 주므로 Impulse 모드 사용
            rb.AddForce(pendingForce, ForceMode.Impulse);
            shouldApplyForce = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 상대방 공과 충돌했을 때
        if (collision.gameObject.CompareTag(opponentTag))
        {
            hitOpponent = true;
        }
        // 타겟 빨간 공과 충돌했을 때 (중복 충돌 방지를 위해 HashSet 사용)
        else if (collision.gameObject.CompareTag(targetTag))
        {
            if (!hitTargets.Contains(collision.gameObject))
            {
                hitTargets.Add(collision.gameObject);
                hitTargetCount = hitTargets.Count;
            }
        }
    }

    public void ResetTurnFlags()
    {
        hitOpponent = false;
        hitTargetCount = 0;
        hitTargets.Clear();
    }
}