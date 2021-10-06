using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveValue
{
    public static int _totalScore = 0;
    public static int _hp = 3;
    public static int _maxHp = 3;
    public static int _nowSceneIndex;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PlayerController theplayerController;
    UIManager theUIManager;
    FloorManager thefloorManager;
    StageManager theStageManager;

    public PlayerController PlayerControllerInstance { get { return theplayerController; } }
    public UIManager UIManagerInstance { get { return theUIManager; } }
    public FloorManager FloorManagerInstance { get { return thefloorManager; } }
    public StageManager StageManagerInstance { get { return theStageManager; } }

    public string[] sceneNames;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        theplayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        theUIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        thefloorManager = GameObject.Find("FloorGroup").GetComponent<FloorManager>();
        theStageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        theStageManager._preSceneName = sceneNames[SaveValue._nowSceneIndex];
        theStageManager._nextSceneName = sceneNames[SaveValue._nowSceneIndex + 1];
    }

    public void SaveTotalScore(int value)
    {
        SaveValue._totalScore = value;
    }

    public void SaveHp(int hp, int maxHp)
    {
        SaveValue._hp = hp;
        SaveValue._maxHp = maxHp;
    }

    public void IncreaseSceneIndex(int index)
    {
        SaveValue._nowSceneIndex = index;
    }

    public int LoadTotalScore()
    {
        return SaveValue._totalScore;
    }

    public int LoadHp()
    {
        return SaveValue._hp;
    }

    public int LoadMaxHp()
    {
        return SaveValue._maxHp;
    }
}
