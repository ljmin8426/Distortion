using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override void OnEnterState() { }

    public override void OnExitState() { }

    public override void OnUpdateState()
    {
        Vector2 input = Controller.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = Controller.MainCamera.forward; camForward.y = 0;
        Vector3 camRight = Controller.MainCamera.right; camRight.y = 0;
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(moveDir);
            Controller.transform.rotation = Quaternion.Slerp(Controller.transform.rotation, rot, Controller.RotationSpeed * Time.deltaTime);
        }

        Vector3 move = moveDir * Controller.MoveSpeed;
        move.y = Controller.VerticalVelocity;
        Controller.Controller.Move(move * Time.deltaTime);

        Controller.Animator.SetFloat("MoveSpeed", input.magnitude);
    }

    public override void OnFixedUpdateState() { }
}
