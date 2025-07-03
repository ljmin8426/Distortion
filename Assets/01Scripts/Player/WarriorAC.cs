using UnityEngine;

public class WarriorAC : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int _speedParam = Animator.StringToHash("MoveSpeed");
    private int _dashTrigger = Animator.StringToHash("IsDash");
    private int _attackTrigger = Animator.StringToHash("IsAttack");

    private void OnEnable()
    {
        AnimationEvents.OnSetSpeed += SetSpeed;
        AnimationEvents.OnDash += PlayDash;
        AnimationEvents.OnAttack += PlayAttackMotion;
    }

    private void OnDisable()
    {
        AnimationEvents.OnSetSpeed -= SetSpeed;
        AnimationEvents.OnDash -= PlayDash;
        AnimationEvents.OnAttack -= PlayAttackMotion;
    }

    private void SetSpeed(float speed)
    {
        animator.SetFloat(_speedParam, speed);
    }

    private void PlayDash()
    {
        animator.SetTrigger(_dashTrigger);
    }

    private void PlayAttackMotion()
    {
        animator.SetTrigger(_attackTrigger);
    }
}
