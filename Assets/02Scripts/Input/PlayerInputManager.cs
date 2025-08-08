using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private InputMapping inputMapping;

    public Vector2 MoveInput { get; private set; }

    public static event Action OnUtil;
    public static event Action OnSwap;
    public static event Action OnDefense;
    public static event Action OnUltimate;
    public static event Action OnSkill;
    public static event Action OnAttack;
    public static event Action OnPause;

    public static event Action OnF1;
    public static event Action OnF2;
    public static event Action OnF3;
    public static event Action OnF4;
    public static event Action OnF5;
    public static event Action OnF6;
    public static event Action OnF7;
    public static event Action OnF8;

    private void Awake()
    {
        inputMapping = new InputMapping();

        inputMapping.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputMapping.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        inputMapping.Player.Jump.performed += _ => OnUtil?.Invoke();
        inputMapping.Player.Util.performed += _ => OnSwap?.Invoke();
        inputMapping.Player.Defense.performed += _ => OnDefense?.Invoke();
        inputMapping.Player.Ultimate.performed += _ => OnUltimate?.Invoke();
        inputMapping.Player.Skill.performed += _ => OnSkill?.Invoke();
        inputMapping.Player.Attack.performed += _ => OnAttack?.Invoke();
        inputMapping.Player.Pause.performed += _ => OnPause?.Invoke();

        inputMapping.Player.F1.performed += _ => OnF1?.Invoke();
        inputMapping.Player.F2.performed += _ => OnF2?.Invoke();
        inputMapping.Player.F3.performed += _ => OnF3?.Invoke();
        inputMapping.Player.F4.performed += _ => OnF4?.Invoke();
        inputMapping.Player.F5.performed += _ => OnF5?.Invoke();
        inputMapping.Player.F6.performed += _ => OnF6?.Invoke();
        inputMapping.Player.F7.performed += _ => OnF7?.Invoke();
        inputMapping.Player.F8.performed += _ => OnF8?.Invoke();
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
