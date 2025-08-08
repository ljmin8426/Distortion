using UnityEngine;

public interface IPlayerSkill
{
    void TryUseSkill(PlayerInputManager input, Transform camTransform, Transform playerTransform, CharacterController controller);
}
