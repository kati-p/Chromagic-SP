using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : CharacterData
{
    [SerializeField]
    private string enemyId;

    [SerializeField]
    private string[] statements;

    [SerializeField]
    private AudioClip enemyTheme;

    [SerializeField]
    private PlayerStatLevelUp playerStatLevelUpWhenWon;

    public string EnemyId { get { return enemyId; } set { enemyId = value; } }

    public string[] Statements { get { return statements; } set { statements = value; } }

    public AudioClip EnemyTheme { get { return enemyTheme; } set { enemyTheme = value; } }  

    public PlayerStatLevelUp PlayerStatLevelUpWhenWon { get { return playerStatLevelUpWhenWon; } set { playerStatLevelUpWhenWon = value; } }
}

[System.Serializable]
public class PlayerStatLevelUp
{
    public float increaseStartedHP;
    public float increaseMP;
    public float increaseMD;
    public float increaseSPD;

    public PlayerStatLevelUp()
    {

    }
}
