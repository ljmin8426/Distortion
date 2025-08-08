using UnityEngine;

public class MoveState : BaseState<PlayerController>
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override void OnEnterState() { }

    public override void OnExitState() { }

    public override void OnUpdateState()
    {
        Vector2 input = controller.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = controller.MainCamera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = controller.MainCamera.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(moveDir);
            controller.transform.rotation = Quaternion.Slerp(
                controller.transform.rotation,
                rot,
                controller.RotationSpeed * Time.deltaTime
            );
        }

        Vector3 move = moveDir * controller.MoveSpeed;
        move.y = controller.VerticalVelocity;
        controller.Controller.Move(move * Time.deltaTime);

        controller.Animator.SetFloat("MoveSpeed", input.magnitude);
    }

    public override void OnFixedUpdateState() { }
}
