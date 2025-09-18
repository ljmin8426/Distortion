using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "GameData/MonsterData")]

public class MonsterData_SO : ScriptableObject
{
    public List<MonsterData> monsterData;
    public List<BossData> bossData;
}
