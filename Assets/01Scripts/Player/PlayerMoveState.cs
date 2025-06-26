using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
    }

    public void Exit() { }

    public void Update()
    {
        Vector2 moveInput = _player.MoveInput;
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 camForward = _player.MainCamera.forward; camForward.y = 0f;
        Vector3 camRight = _player.MainCamera.right; camRight.y = 0f;
        Vector3 moveDir = camForward.normalized * inputDir.z + camRight.normalized * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRot, _player.RotationSpeed * Time.deltaTime);
        }

        Vector3 move = moveDir * _player.MoveSpeed;
        move.y = _player.VerticalVelocity;
        _player.Controller.Move(move * Time.deltaTime);

        _player.Animator.SetFloat("MoveSpeed", moveInput.magnitude);
    }
}
