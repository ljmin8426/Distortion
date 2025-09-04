using UnityEngine;

public class TwoHandSword : BaseWeapon, IMeleeWeapon
{
    private BoxCollider meleeArea;
    private TrailRenderer trailRenderer;
    private ColliderChecker colliderChecker;

    private void Awake()
    {
        meleeArea = GetComponentInChildren<BoxCollider>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        colliderChecker = GetComponentInChildren<ColliderChecker>();

        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }

    public void EnableMelee()
    {
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        AudioManager.Instance.PlaySoundFXClip(AttackSound, transform, 1f);
    }

    public void DisableMelee()
    {
        meleeArea.enabled = false;
        trailRenderer.enabled = false;
        colliderChecker.ClearDamagedTargets();
    }

    public override void Attack() { }
    public override void Skill() { }
}

