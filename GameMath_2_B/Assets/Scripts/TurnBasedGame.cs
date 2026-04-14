using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedGame : MonoBehaviour
{
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    int turn = 0;
    bool rareItemObtained = false;
    float cumulativeBonus = 0f; // 턴마다 쌓일 추가 확률

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    // 버튼에 연결할 메인 시뮬레이션 함수
    public void StartSimulation()
    {
        rareItemObtained = false;
        turn = 0;
        cumulativeBonus = 0f; // 확률 초기화

        while (!rareItemObtained)
        {
            turn++;
            SimulateTurn();

            // 매 턴이 끝날 때마다 확률 5%(0.05)씩 증가
            cumulativeBonus += 0.05f;
        }

        Debug.Log($"<color=cyan>최종 결과: 레어 아이템을 {turn} 턴 만에 획득했습니다! (최종 보너스 확률: {cumulativeBonus * 100}%)</color>");
    }

    void SimulateTurn()
    {
        Debug.Log($"--- Turn {turn} (보너스 확률: +{cumulativeBonus * 100:F0}%) ---");

        int enemyCount = SamplePoisson(poissonLambda);
        Debug.Log($"적 등장 : {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    Debug.Log($" 크리티컬 hit! {damage:F1}");
                }
                else
                    Debug.Log($" 일반 hit! {damage:F1}");

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");

                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                Debug.Log($"보상: {reward}");

                // 기본 확률 20% + 누적 보너스 확률 적용
                float finalRareChance = 0.2f + cumulativeBonus;

                if (reward == "Weapon" && Random.value < finalRareChance)
                {
                    rareItemObtained = true;
                    Debug.Log("<color=yellow>레어 무기 획득!</color>");
                }
                else if (reward == "Armor" && Random.value < finalRareChance)
                {
                    rareItemObtained = true;
                    Debug.Log("<color=yellow>레어 방어구 획득!</color>");
                }
            }
        }
    }

    // --- 분포 샘플 함수들 (기존과 동일) ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}