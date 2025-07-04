using UnityEngine;

public interface IPlayerSkill
{
    void TryUseSkill(InputManager input, Transform camTransform, Transform playerTransform, CharacterController controller);
}
