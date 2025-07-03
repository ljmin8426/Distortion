using UnityEngine;

public interface IPlayerSkill
{
    void TryUseSkill(InputManager input, WarriorAC ac, Transform camTransform, Transform playerTransform, CharacterController controller);
}
