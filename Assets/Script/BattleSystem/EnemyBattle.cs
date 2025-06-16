using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattle : CharacterBattle
{
    private string enemyId;

    public string EnemyId {  get { return enemyId; } }

    [SerializeField]
    private List<Magic> enemyMagics = new List<Magic>();

    public override void SetUp()
    {
        EnemyData enemyData = GameData.Instance.EnemyEncouter;

        this.characterName = enemyData.CharacterName;
        this.enemyId = enemyData.EnemyId;
        this.MP = enemyData.MagicPower;
        this.MD = enemyData.MagicDefense;
        this.SPD = enemyData.Speed;
        enemyMagics = enemyData.Magics;

        statusWindow.SetUp(characterName, MP, MD, SPD, enemyMagics.Count);

        Color rndColor = RandomMyColor();
        HP.SetUp(enemyData.StartedHealthPower, enemyData.StartedHealthPower, rndColor);

        if (enemyMagics.Count <= 0)
        {
            throw new System.Exception("Please set enemy's magic");
        }

        Animator animator = characterImage.GetComponent<Animator>();
        animator.runtimeAnimatorController = enemyData.CharacterAnimatorController;
    }

    public Color RandomMyColor()
    {  
        int rndIndex = Random.Range(0, enemyMagics.Count);
        Color randomColor = enemyMagics[rndIndex].Color;

        return randomColor;
    }

    public Magic RandomMagicCanUseByAP()
    {
        int AP = this.AP.GetAP();

        Magic[] temp = enemyMagics
            .Where(magic => magic.Cost <= AP)
            .ToArray();

        int rndIndex = Random.Range(0, temp.Length);
        Magic randomMagic = temp[rndIndex];

        return randomMagic;
    }

    public int GetMagicCountCanUseByAP()
    {
        int AP = this.AP.GetAP();

        Magic[] temp = enemyMagics
            .Where( magic => magic.Cost <= AP)
            .ToArray();

        return temp.Length;
    }

    private void Awake()
    {
        
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
