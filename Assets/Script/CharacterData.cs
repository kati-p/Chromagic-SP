using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private string characterName;
    
    [SerializeField]
    private float startedHealthPower;

    [SerializeField]
    private List<Magic> magics = new List<Magic>();

    [SerializeField]
    private float magicPower;

    [SerializeField]
    private float magicDefense;

    [SerializeField]
    private float speed;

    [SerializeField]
    private RuntimeAnimatorController characterBattleAnimatorController;

    public string CharacterName { get { return characterName; } set { characterName = value; } }

    public float StartedHealthPower {  get { return startedHealthPower; } set { startedHealthPower = value; } }

    public List<Magic> Magics { get { return magics; } }

    public float MagicPower { get {return magicPower; } set { magicPower = value; } }

    public float MagicDefense { get { return magicDefense; } set { magicDefense = value; } }

    public float Speed { get { return speed; } set { speed = value; } }

    public RuntimeAnimatorController CharacterAnimatorController { get { return characterBattleAnimatorController; } }

}
