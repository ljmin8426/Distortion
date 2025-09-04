using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public int curEXP;
    public int maxHP;
    public int curHP;
    public int maxEP;
    public int curEP;

    public int fragment;
    public int uidCounter;

    // 인벤토리, 스탯, 특전 연결
}

public enum SCENE_NAME
{
    MainScene,
    LoadScene,
    WaveScene
}


public class GameManager : Singleton<GameManager>
{
    private PlayerData playerData;
    private string dataPath;

    public PlayerData PlayerData => playerData;

    protected override void Awake()
    {
        base.Awake();
        dataPath = Application.persistentDataPath + "/Save";
    }

    public void CreateUserData(string newNickName)
    {
        playerData = new PlayerData();

        playerData.name = newNickName;
        playerData.level = 1;
        playerData.curEXP = 0;
        playerData.curHP = playerData.maxHP = 100;
        playerData.curEP = playerData.maxEP = 100;
        playerData.fragment = 10000;
        playerData.uidCounter = 0;
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData);

        // 암호화 

        File.WriteAllText(dataPath, data);
    }

    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            // 복호화
            playerData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        return false;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public bool TryGetPlayerData()
    {
        if (File.Exists(dataPath))
        {
            return LoadData(); // 데이터 로드 성공
        }
        return false;
    }

    private SCENE_NAME nextSceneName;
    public SCENE_NAME nextScene => nextSceneName;
    public void AsyncLoadNextScene(SCENE_NAME nextScene)
    {
        nextSceneName = nextScene;
        SceneManager.LoadScene(SCENE_NAME.LoadScene.ToString());
    }
}
