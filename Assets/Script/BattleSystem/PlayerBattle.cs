using UnityEngine;

public class PlayerBattle : CharacterBattle
{
    
    public override void SetUp()
    {
        CharacterData playerData = GameData.Instance.PlayerData;

        this.characterName = playerData.CharacterName;
        this.MP = playerData.MagicPower;
        this.MD = playerData.MagicDefense;
        this.SPD = playerData.Speed;

        statusWindow.SetUp(characterName, MP, MD, SPD, playerData.Magics.Count);

        int rndIndex = Random.Range(0, playerData.Magics.Count);
        Color rndColor = playerData.Magics[rndIndex].Color;
        HP.SetUp(playerData.StartedHealthPower, playerData.StartedHealthPower, rndColor);

        Animator animator = characterImage.GetComponent<Animator>();
        animator.runtimeAnimatorController = playerData.CharacterAnimatorController;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
