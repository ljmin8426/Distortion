using UnityEngine;

public class TwoHandSword : BaseWeapon, IMeleeWeapon
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        trailRenderer.enabled = false;
    }

    public void EnableMelee()
    {
        trailRenderer.enabled = true;
    }

    public void DisableMelee()
    {
        trailRenderer.enabled = false;
    }

    public override void Skill() { }
    public override void Attack() { }
}
