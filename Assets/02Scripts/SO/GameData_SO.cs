using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/Game Data")]

public class GameData_SO : ScriptableObject
{
    public List<WeaponItemData> weaponData;
    public List<MonsterData> monsterData;
    public List<EXPTable> expTableData;
    public List<BossData> bossData;
}
