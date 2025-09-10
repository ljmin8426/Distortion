using UnityEngine;

public class TwoHandSword : BaseWeapon
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

    public override void AttackStart()
    {
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        AudioManager.Instance.PlaySoundFXClip(AttackSound, transform, 1f);
    }

    public override void AttackEnd()
    {
        meleeArea.enabled = false;
        trailRenderer.enabled = false;
        colliderChecker.ClearDamagedTargets();
    }
}

