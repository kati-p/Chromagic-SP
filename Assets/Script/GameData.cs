using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData : MonoBehaviour
{
    private static GameData instance;

    public static GameData Instance {  
        get {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameData>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    instance = gameObject.AddComponent<GameData>();
                    gameObject.name = "GameData";
                    DontDestroyOnLoad(gameObject);
                } 
            }
            return instance;
        } 
    }

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Data save to file
    [SerializeField]
    private PlayerData defaultPlayerData;

    [SerializeField]
    private PlayerData playerData;

    private Dictionary<string, bool> enemiesDefeated = new Dictionary<string, bool>();

    private int gameProgress = 0;    // TODO int to enum state

    private Vector2 playerPosition = Vector2.zero;

    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    public Dictionary<string, bool> EnemiesDefeated { get {  return enemiesDefeated; } }

    public int GameProgress { get { return gameProgress; } set { gameProgress = value; } }

    public Vector2 PlayerPosition { get { return  playerPosition; } set { playerPosition = value; } }


    // Data use in runtime

    [SerializeField]
    private EnemyData enemyDataEncouter;

    private Vector2 enemyEncouterPosition = Vector2.zero;

    public EnemyData EnemyEncouter { get { return enemyDataEncouter; } set { enemyDataEncouter = value; } }

    public Vector2 EnemyEncouterPosition { get { return enemyEncouterPosition; } set { enemyEncouterPosition = value; } }

    public void ResetGameData()
    {
        SaveLoadManager.DeleteGame();

        SetUpDefault();
    }

    public void SetUpDefault()
    {
        this.enemiesDefeated = new Dictionary<string, bool>();
        this.gameProgress = 0;
        this.playerPosition = Vector2.zero;

        this.playerData.StartedHealthPower = defaultPlayerData.StartedHealthPower;
        this.playerData.MagicPower = defaultPlayerData.MagicPower;
        this.playerData.MagicDefense = defaultPlayerData.MagicDefense;
        this.playerData.Speed = defaultPlayerData.Speed;
    }

    public void SaveGame()
    {
        SaveLoadManager.SaveGame(instance);
        
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        GameSaveData data = SaveLoadManager.LoadGame();

        if (data == null) return;

        this.enemiesDefeated = new Dictionary<string, bool>(data.enemiesDefeated);
        this.gameProgress = data.gameProgresss;
        this.playerPosition = new Vector2(data.playerPositionX, data.playerPositionY);

        if (data.playerStartedHP > 0) this.playerData.StartedHealthPower = data.playerStartedHP;
        if (data.playerMP > 0) this.playerData.MagicPower = data.playerMP;
        if (data.playerMD > 0) this.playerData.MagicDefense = data.playerMD;
        if (data.playerSPD > 0) this.playerData.Speed = data.playerSPD;
    }
}
