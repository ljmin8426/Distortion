using UnityEngine;

public class MonsterAttackCollider : MonoBehaviour, IAttackTrigger
{
    private Collider col;
    private MonsterBase monster;


    private void Awake()
    {
        col = GetComponent<Collider>();
        monster = GetComponentInParent<MonsterBase>();
        col.enabled = false;
    }
    public void Attack()
    {
        col.enabled = true;
        AudioManager.Instance.PlaySoundFXClip(monster.AttackSoundClip, monster.Owner.transform, 1f);
    }

    public void HitEnd()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!col.enabled) return;

        if(other.CompareTag("Enemy")) return;

        if (other.TryGetComponent<IDamageable>(out var damaged))
        {
            damaged.TakeDamage(monster.MonsterData.attackDamage, monster.Owner);
        }
    }
}
