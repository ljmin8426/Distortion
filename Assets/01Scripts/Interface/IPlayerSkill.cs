using UnityEngine;

public interface IPlayerSkill
{
    void TryUseSkill(PlayerInputController input, PlayerAnimatorController animator, Transform camTransform, Transform playerTransform, CharacterController controller);
}
