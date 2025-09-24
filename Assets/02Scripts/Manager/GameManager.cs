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
}

public enum SCENE_NAME
{
    MainScene,
    LoadScene,
    WaveScene
}


public class GameManager : Singleton<GameManager>
{
    private SCENE_NAME nextSceneName;
    public SCENE_NAME nextScene => nextSceneName;
    public void AsyncLoadNextScene(SCENE_NAME nextScene)
    {
        nextSceneName = nextScene;
        SceneManager.LoadScene(SCENE_NAME.LoadScene.ToString());
    }
}
