 using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Data")]
public class EnemyDateSO : ScriptableObject
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

    [Header("Reward")]
    public int expReward;
}
