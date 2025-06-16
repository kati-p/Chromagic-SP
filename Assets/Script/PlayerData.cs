using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : CharacterData
{
    private int skillPoint;

    public int SkillPoint {  get { return skillPoint; } set { skillPoint = value; } }
}
