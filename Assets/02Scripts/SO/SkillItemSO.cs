using UnityEngine;

[CreateAssetMenu(fileName = "SkillItem",menuName = "Item/Skill Item")]
public class SkillItemSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public ITEM_TYPE itemType = ITEM_TYPE.Skill;
    public GameObject skillPrefab; 
}
