using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerController _player;
    private float _timer;
    private readonly float _attackDuration = 0.5f;

    public void Enter(PlayerController player)
    {
        _player = player;

        // 회전
        Vector3 lookDir = _player.GetMouseWorldDirection();
        _player.TryLook(lookDir);

        // 애니메이션
        _player.Animator.SetTrigger("IsAttack");

        // 무기 공격
        _player.TryAttack();

        _timer = _attackDuration;
    }

    public void Exit() { }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _player.ChangeState(new PlayerMoveState());
        }
    }
}
