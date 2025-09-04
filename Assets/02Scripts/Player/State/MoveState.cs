using UnityEngine;

public class MoveState : BaseState<PlayerController>
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override bool CanEnter()
    {
        // 공격 상태일 때도 이동 제한을 걸고 싶으면 조건 추가 가능
        // 현재는 항상 이동 가능으로 처리
        return true;
    }

    public override void OnEnterState()
    {
        // 이동 상태 진입 시 초기화가 필요하면 작성
    }

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

        owner.Animator.SetFloat("MoveSpeed", input.magnitude);
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }
}
