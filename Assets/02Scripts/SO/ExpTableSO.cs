using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ExpTable", menuName = "GameData/Exp Table")]
public class ExpTableSO : ScriptableObject
{
    [System.Serializable]
    public class LevelExpData
    {
        [Header("기본 정보")]
        public int level;             // 레벨
        public float requiredExp;     // 다음 레벨까지 필요한 경험치

        [Header("추가 보상")]
        public float bonusHP;         // 레벨업 시 추가 HP
        public float bonusATK;        // 레벨업 시 추가 ATK
        public bool unlockNewSkill;   // 스킬 해금 여부
    }

    [Tooltip("레벨별 필요 경험치 + 보상 테이블")]
    public LevelExpData[] expTable;

    /// <summary>
    /// 해당 레벨의 필요 경험치 반환
    /// </summary>
    public float GetExpRequired(int level)
    {
        if (level <= 0) return 0;

        if (level > expTable.Length) // 최대 레벨 초과 시
        {
            Debug.LogWarning("[ExpTableSO] 요청한 레벨이 최대 레벨을 초과했습니다.");
            return 0; // 초과하면 더 이상 경험치 필요 없음
        }

        return expTable[level - 1].requiredExp;
    }

    /// <summary>
    /// 최대 레벨 여부 확인
    /// </summary>
    public bool IsMaxLevel(int level)
    {
        return level >= expTable.Length;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 기본 경험치 테이블 자동 생성
    /// </summary>
    [ContextMenu("Generate Default Table")]
    public void GenerateDefaultTable()
    {
        int maxLevel = 20; // 기본 최대 레벨
        expTable = new LevelExpData[maxLevel];

        float baseExp = 10f;       // 시작 경험치
        float step = 5f;           // 레벨당 증가량
        float gapMultiplier = 2f;  // 5레벨마다 간극 배율

        for (int i = 0; i < maxLevel; i++)
        {
            expTable[i] = new LevelExpData();
            expTable[i].level = i + 1;

            // 경험치 계산
            float required = baseExp + (step * i);
            if ((i + 1) % 5 == 0) // 5레벨마다 간극
                required *= gapMultiplier;

            expTable[i].requiredExp = required;

            // 예시 보너스 값
            expTable[i].bonusHP = (i + 1) * 2;
            expTable[i].bonusATK = (i + 1) * 0.5f;
            expTable[i].unlockNewSkill = ((i + 1) % 10 == 0);
        }

        EditorUtility.SetDirty(this);
        Debug.Log("[ExpTableSO] 기본 경험치 테이블 생성 완료!");
    }
#endif
}
