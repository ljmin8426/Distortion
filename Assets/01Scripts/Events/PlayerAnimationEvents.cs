using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponentInParent<PlayerController>();
    }

    public void OnDash()
    {
        var dashState = _controller.GetState<DashState>(PLAYER_STATE.Dash);
    }

    public void EndDash() // 애니메이션 마지막에 호출
    {
        _controller.ChangeToState(PLAYER_STATE.Move);
    }


    public void OnAttack()
    {
        if (_controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.EnableMelee();
        }
    }

    public void EndAttack()
    {
        if (_controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.DisableMelee();
        }
        _controller.ChangeToState(PLAYER_STATE.Move);
    }

}
