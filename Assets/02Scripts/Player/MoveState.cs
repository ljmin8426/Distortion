using UnityEngine;

public class MoveState : BaseState<PlayerController>
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override void OnEnterState() { }

    public override void OnExitState() { }

    public override void OnUpdateState()
    {
        Vector2 input = Owner.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = Owner.MainCamera.forward; camForward.y = 0;
        Vector3 camRight = Owner.MainCamera.right; camRight.y = 0;
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(moveDir);
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, rot, Owner.RotationSpeed * Time.deltaTime);
        }

        Vector3 move = moveDir * Owner.MoveSpeed;
        move.y = Owner.VerticalVelocity;
        Owner.Controller.Move(move * Time.deltaTime);

        Owner.Animator.SetFloat("MoveSpeed", input.magnitude);
    }

    public override void OnFixedUpdateState() { }
}
