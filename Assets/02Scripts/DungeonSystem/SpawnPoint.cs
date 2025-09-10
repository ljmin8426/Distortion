using UnityEngine;

public enum Monster_Type
{
    Melee,
    Hammer,
    Polearm
}


public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Monster_Type monster_Name = Monster_Type.Melee;

    public Monster_Type Monster_Type => monster_Name;
}
