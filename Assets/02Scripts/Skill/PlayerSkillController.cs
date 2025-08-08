using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private BaseSkillManager skillManager;

    private void OnEnable()
    {
        PlayerInputManager.OnSkill += AttackSkill;
        PlayerInputManager.OnDefense += HandleDefense;
        PlayerInputManager.OnUltimate += HandleUltimate;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnSkill -= AttackSkill;
        PlayerInputManager.OnDefense -= HandleDefense;
        PlayerInputManager.OnUltimate -= HandleUltimate;
    }

    private void Awake()
    {
        skillManager = GetComponent<BaseSkillManager>();
    }

    private void HandleDefense()
    {
        skillManager.UseSkill(1);
    }

    private void AttackSkill()
    {
        skillManager.UseSkill(2);
    }

    private void HandleUltimate()
    {
        skillManager.UseSkill(3);
    }
}
