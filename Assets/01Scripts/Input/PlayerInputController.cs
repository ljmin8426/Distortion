using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Vector2 move;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isSprint;

    public Vector2 Move => move;
    public bool IsJump => isJump;
    public bool IsSprint => isSprint;


    public void OnMove(InputValue inputValue)
    {
        InputMove(inputValue.Get<Vector2>());
    }

    public void OnJump(InputValue inputValue)
    {
        InputJump(inputValue.isPressed);
    }

    public void OnSprint(InputValue inputValue)
    {
        InputSprint(inputValue.isPressed);
    }

    private void InputMove(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    private void InputJump(bool newJumpState)
    {
        isJump = newJumpState;
    }

    private void InputSprint(bool newSprintState)
    {
        isSprint = newSprintState;
    }
}
