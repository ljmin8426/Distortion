using UnityEngine;

public class MoveState : BaseState<PlayerCtrl>
{
    public MoveState(PlayerCtrl controller) : base(controller) { }

    public override bool CanEnter()
    {

        return true;
    }

    public override void OnEnterState() { }

    public override void OnUpdateState()
    {
        Vector2 input = owner.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = owner.MainCamera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = owner.MainCamera.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(moveDir);
            owner.transform.rotation = Quaternion.Slerp(
                owner.transform.rotation,
                rot,
                owner.RotationSpeed * Time.deltaTime
            );
        }

        Vector3 move = moveDir * owner.MoveSpeed;
        move.y = owner.VerticalVelocity;
        owner.Controller.Move(move * Time.deltaTime);

        owner.Animator.SetFloat(owner.Speed, input.magnitude);
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }
}
