using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/Data")]
public class SkillDataSO : ScriptableObject
{
    public int cooldown;
    public int manaCost;

    public SKILL_TYPE skillType;

    public GameObject effectPrefab;
    public AudioClip soundEffect;
    public Sprite SkillIcon;
}

