using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "Game Data/Class Data")]
public class PlayerStatSO : ScriptableObject
{
    [Header("Class")]
    public string className;
    public string classDescription;

    [Header("Default Stat")]
    public float hp;
    public float ep;
    public float atk;
    public float def;
    public float ag;
}
