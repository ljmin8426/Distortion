using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Datas", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public int maxHP;
    public float moveSpeed;

    [Header("Combat")]
    public int attackPower;
    public float attackRange;
    public float attackCooldown;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float chaseStopRange = 15f;

    [Header("Reward")]
    public int expReward;
}
