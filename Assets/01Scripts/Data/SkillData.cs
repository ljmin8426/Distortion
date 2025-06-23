using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int cooldown;
    public int manaCost;

    public GameObject effectPrefab;
    public AudioClip soundEffect;
    public AnimationClip animationClip;
}

