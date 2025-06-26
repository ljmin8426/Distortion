using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSkillController : MonoBehaviour
{
    private BaseSkillManager skillManager;
    private WeaponManager weaponManager;

    private void OnEnable()
    {
        PlayerInputController.OnSkill += HandleSkill;
        PlayerInputController.OnDefense += HandleDefense;
        PlayerInputController.OnUltimate += HandleUltimate;
    }

    private void OnDisable()
    {
        PlayerInputController.OnSkill -= HandleSkill;
        PlayerInputController.OnDefense -= HandleDefense;
        PlayerInputController.OnUltimate -= HandleUltimate;
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
