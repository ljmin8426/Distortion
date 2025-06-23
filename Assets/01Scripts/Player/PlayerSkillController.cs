using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSkillController : MonoBehaviour
{
    private BaseSkillManager skillManager;

    private void OnEnable()
    {
        PlayerInputController.OnSkill += HandleSkill;
        PlayerInputController.OnUltimate += HandleUltimate;
        PlayerInputController.OnUtil += HandleUtil;
        PlayerInputController.OnDefense += HandleDefense;
        PlayerInputController.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        PlayerInputController.OnSkill -= HandleSkill;
        PlayerInputController.OnUtil -= HandleUtil;
        PlayerInputController.OnDefense -= HandleDefense;
        PlayerInputController.OnUltimate -= HandleUltimate;
    }

    private void Awake()
    {
        skillManager = GetComponent<BaseSkillManager>();
    }

    private void HandleAttack()
    {
        AnimationEvents.OnAttack?.Invoke();
    }

    private void HandleSkill()
    {
        skillManager.UseSkill(0);
    }

    private void HandleUltimate()
    {

    }

    private void HandleUtil()
    {

    }
    
    private void HandleDefense()
    {

    }
}
