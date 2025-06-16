using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoadManager
{
    private static string savePath = Application.persistentDataPath + "/save.dat";
    
    public static void SaveGame(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        GameSaveData data = new GameSaveData(gameData);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static GameSaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            GameSaveData data = bf.Deserialize(stream) as GameSaveData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Tried to load game, but data was not found at path: " + savePath);

            return new GameSaveData(); 
        }
        
        
    }

    public static void DeleteGame()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        else
        {
            Debug.LogWarning("Tried to delete game, but data was not found at path: " + savePath);
        }
    }

    public static bool IsSaveFileExist()
    {
        return File.Exists(savePath);
    }
}

[Serializable]
public class GameSaveData
{
    public Dictionary<string, bool> enemiesDefeated = new Dictionary<string, bool>();
    public int gameProgresss = 0;
    public float playerPositionX = 0;
    public float playerPositionY = 0;
    public float playerStartedHP = 0;
    public float playerMP = 0;
    public float playerMD = 0;
    public float playerSPD = 0;

    public GameSaveData()
    {

    }

    public GameSaveData(GameData gameData)
    {
        this.enemiesDefeated = new Dictionary<string, bool>(gameData.EnemiesDefeated);
        this.gameProgresss = gameData.GameProgress;
        this.playerPositionX = gameData.PlayerPosition.x;
        this.playerPositionY = gameData.PlayerPosition.y;
        this.playerStartedHP = gameData.PlayerData.StartedHealthPower;
        this.playerMP = gameData.PlayerData.MagicPower;
        this.playerMD = gameData.PlayerData.MagicDefense;
        this.playerSPD = gameData.PlayerData.Speed;
    }
}
