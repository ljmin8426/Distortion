using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ExpTable", menuName = "Game Data/Exp Table")]
public class ExpTableSO : ScriptableObject
{
    [System.Serializable]
    public class LevelExpData
    {
        [Header("기본 정보")]
        public int level;            
        public float requiredExp;
    }

    [Tooltip("레벨별 필요 경험치 + 보상 테이블")]
    public LevelExpData[] expTable;

    public float GetExpRequired(int level)
    {
        if (level <= 0) return 0;

        if (level > expTable.Length)
        {
            Debug.LogWarning("[ExpTableSO] 요청한 레벨이 최대 레벨을 초과했습니다.");
            return 0;
        }

        return expTable[level - 1].requiredExp;
    }

    public bool IsMaxLevel(int level)
    {
        return level >= expTable.Length;
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Default Table")]
    public void GenerateDefaultTable()
    {
        int maxLevel = 20;
        expTable = new LevelExpData[maxLevel];

        float baseExp = 10f;     
        float step = 5f;         
        float gapMultiplier = 2f;

        for (int i = 0; i < maxLevel; i++)
        {
            expTable[i] = new LevelExpData();
            expTable[i].level = i + 1;

            float required = baseExp + (step * i);
            if ((i + 1) % 5 == 0)
                required *= gapMultiplier;

            expTable[i].requiredExp = required;
        }

        EditorUtility.SetDirty(this);
        Debug.Log("[ExpTableSO] 기본 경험치 테이블 생성 완료");
    }
#endif
}
