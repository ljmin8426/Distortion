using UnityEngine;

public class PlayerDashState : IPlayerState
{
    private PlayerController _player;
    private float _timer;
    private float _duration = 0.3f;
    private float _dashSpeed = 20f;
    private Vector3 _dashDirection;

    public void Enter(PlayerController player)
    {
        _player = player;
        _dashDirection = player.transform.forward;
        _player.Animator.SetTrigger("IsDash");
        _timer = _duration;
    }

    public void Exit() { }

    public void Update()
    {
        Vector3 move = _dashDirection * _dashSpeed;
        move.y = _player.VerticalVelocity;
        _player.Controller.Move(move * Time.deltaTime);

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _player.ChangeState(new PlayerMoveState());
        }
    }
}
