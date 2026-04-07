using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageSimlator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0);
    }
    
    private void ResetData()
    {         
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }
    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검",0.2f, 0.3f, 2.0f);
        }
        else 
        { 
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    public void OnAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamdage(baseDamage, sd);

        bool isCrit = Random.value < critRate;
        float finalDamage = isCrit ? normalDamage * critMult : normalDamage;

        attackCount++;
        totalDamage += finalDamage;

        string critMark = isCrit ? "<color=red>[치명타!]</color>" : "";
        logDisplay.text = string.Format("{0}데미지 : {1:F1}", critMark, finalDamage);
        UpdateUI();
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}|n 기본 데미지: {2} / 치명타: {3}% (x{4})", 
            level, weaponName, baseDamage , critRate * 100, critMult );

        rangeDisplay.text = string.Format("예상 일반 데미지 범위: [{0:F1} ~ {1:F1}]", 
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수:{1}\n평균 DPA: {2:F1}",
           totalDamage,attackCount, dpa);
    }

    private float GetNormalStdDevDamdage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; 
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    public void OnAttackThousandTimes()
    {
        // 성능을 위해 루프 안에서는 UI 업데이트를 하지 않고, 계산이 끝난 후 한 번만 업데이트합니다.
        for (int i = 0; i < 1000; i++)
        {
            // 1. 박스-뮬러 변환으로 일반 데미지 계산
            float u1 = 1.0f - Random.value;
            float u2 = 1.0f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            float currentDamage = baseDamage + (baseDamage * stdDevMult) * randStdNormal;

            // 2. 치명타 판정 (Random.value가 0~1 사이이므로 critRate와 비교)
            if (Random.value < critRate)
            {
                currentDamage *= critMult;
            }

            // 3. 누적 데이터 기록
            totalDamage += currentDamage;
            attackCount++;
        }

        // 1000번 계산이 모두 끝난 후 UI를 갱신합니다.
        logDisplay.text = "<color=yellow>[연속 공격]</color> 1,000회 완료!";
        UpdateUI();
    }
}
