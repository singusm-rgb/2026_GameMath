using UnityEngine;

public class AutoTargetingSystem : MonoBehaviour
{
    public Transform currentTarget; // 현재 타겟팅 중인 적
    public Camera playerCamera; // 플레이어 카메라 참조
    public float rotationSpeed = 5f; // 카메라가 타겟을 따라가는 속도

    void Update()
    {
        // --- 타겟팅 처리 ---
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭
        {
            HandleRightClick();
        }

        // --- 카메라 따라가기 처리 ---
        if (currentTarget != null)
        {
            // 타겟을 부드럽게 바라보도록 합니다.
            Vector3 direction = currentTarget.position - playerCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleRightClick()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 적을 명중했는지 확인
            if (hit.collider.CompareTag("Enemy")) // 적 프리팹에 "Enemy" 태그가 설정되어 있는지 확인하세요.
            {
                currentTarget = hit.transform;
                Debug.Log("타겟팅: " + currentTarget.name);
                // 여기서 조준점을 생성하거나 활성화할 수 있습니다.
            }
            else
            {
                // 적이 아닌 다른 것을 클릭했습니다. 현재 타겟을 해제합니다.
                Untarget();
            }
        }
        else
        {
            // 빈 공간을 클릭했습니다. 현재 타겟을 해제합니다.
            Untarget();
        }
    }

    public void Untarget()
    {
        currentTarget = null;
        Debug.Log("타겟 해제됨.");
        // 여기서 조준점을 파괴하거나 비활성화할 수 있습니다.
    }
}
