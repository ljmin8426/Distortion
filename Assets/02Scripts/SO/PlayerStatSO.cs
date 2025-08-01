using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player Data")]
public class PlayerStatSO : ScriptableObject
{
    [Header("클래스")]
    public string className;
    public string classDescription;

    [Header("기본 스탯")]
    public float HP;
    public float EP;
    public float Atk;
    public float def;
    public float Ag;
}
