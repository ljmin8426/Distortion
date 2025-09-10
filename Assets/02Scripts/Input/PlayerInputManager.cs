using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private InputMapping inputMapping;

    public Vector2 MoveInput { get; private set; }

    public static event Action OnAttack;

    public static event Action OnUtil;
    public static event Action OnSwap;
    public static event Action OnDefense;
    public static event Action OnUltimate;
    public static event Action OnSkill;


    private void Awake()
    {
        inputMapping = new InputMapping();

        inputMapping.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputMapping.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        inputMapping.Player.Attack.performed += _ => OnAttack?.Invoke();
        inputMapping.Player.Jump.performed += _ => OnUtil?.Invoke();
        inputMapping.Player.Util.performed += _ => OnSwap?.Invoke();
        inputMapping.Player.Defense.performed += _ => OnDefense?.Invoke();
        inputMapping.Player.Ultimate.performed += _ => OnUltimate?.Invoke();
        inputMapping.Player.Skill.performed += _ => OnSkill?.Invoke();
    }

    private void OnEnable()
    {
        inputMapping.Player.Enable();
    }

    private void OnDisable()
    {
        inputMapping.Player.Disable();
    }
}
