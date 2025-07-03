using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputMapping _controls;

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
        _controls = new InputMapping();

        // Move
        _controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        // Button Inputs
        _controls.Player.Jump.performed += _ => OnUtil?.Invoke();
        _controls.Player.Util.performed += _ => OnSwap?.Invoke();
        _controls.Player.Defense.performed += _ => OnDefense?.Invoke();
        _controls.Player.Ultimate.performed += _ => OnUltimate?.Invoke();
        _controls.Player.Skill.performed += _ => OnSkill?.Invoke();
        _controls.Player.Attack.performed += _ => OnAttack?.Invoke();
        _controls.Player.Pause.performed += _ => OnPause?.Invoke();

        _controls.Player.F1.performed += _ => OnF1?.Invoke();
        _controls.Player.F2.performed += _ => OnF2?.Invoke();
        _controls.Player.F3.performed += _ => OnF3?.Invoke();
        _controls.Player.F4.performed += _ => OnF4?.Invoke();
        _controls.Player.F5.performed += _ => OnF5?.Invoke();
        _controls.Player.F6.performed += _ => OnF6?.Invoke();
        _controls.Player.F7.performed += _ => OnF7?.Invoke();
        _controls.Player.F8.performed += _ => OnF8?.Invoke();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }
}
