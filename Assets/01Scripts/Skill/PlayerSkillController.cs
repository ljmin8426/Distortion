using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSkillController : MonoBehaviour
{
    private BaseSkillManager skillManager;

    private void OnEnable()
    {
        InputManager.OnSkill += HandleSkill;
        InputManager.OnDefense += HandleDefense;
        InputManager.OnUltimate += HandleUltimate;
    }

    private void OnDisable()
    {
        InputManager.OnSkill -= HandleSkill;
        InputManager.OnDefense -= HandleDefense;
        InputManager.OnUltimate -= HandleUltimate;
    }

    private void Awake()
    {
        skillManager = GetComponent<BaseSkillManager>();
    }


    private void HandleSkill()
    {
        skillManager.UseSkill(0);
    }

    private void HandleUltimate()
    {

    }
    
    private void HandleDefense()
    {

    }
}
