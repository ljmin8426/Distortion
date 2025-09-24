using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private InputMapping inputMapping;

    public Vector2 MoveInput { get; private set; }

    public static event Action OnAttack;

    public static event Action OnUtil;

    public static event Action OnSkillQ;
    public static event Action OnSkillE;


    private void Awake()
    {
        inputMapping = new InputMapping();

        inputMapping.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputMapping.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        inputMapping.Player.Attack.performed += _ => OnAttack?.Invoke();
        inputMapping.Player.Util.performed += _ => OnUtil?.Invoke();
        inputMapping.Player.QSkill.performed += _ => OnSkillQ?.Invoke();
        inputMapping.Player.ESkill.performed += _ => OnSkillE?.Invoke();
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
