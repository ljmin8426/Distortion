using UnityEngine;

[CreateAssetMenu(fileName = "SkillItem",menuName = "Item/Skill Item")]
public class SkillItemSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public Item_Type itemType = Item_Type.Skill;
    public GameObject skillPrefab; 
}
