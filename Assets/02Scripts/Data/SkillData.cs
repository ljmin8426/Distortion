using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public int cooldown;
    public int manaCost;

    public SKILL_TYPE skillType;

    public GameObject effectPrefab;
    public AudioClip soundEffect;
    public AnimationClip animationClip;
    public Sprite SkillIcon;
}

