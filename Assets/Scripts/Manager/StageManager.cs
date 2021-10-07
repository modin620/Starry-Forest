using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] int _index;
    public string _preSceneName;
    public string _nextSceneName;

    bool _stop;
    bool _end;

    public bool end { get { return _end; } }
    public bool stop { get { return _stop; } }

    void SaveValue(bool victory)
    {
        GameManager.instance.SaveTotalScore(GameManager.instance.UIManagerInstance.ScoreInstance.totalScore);
        GameManager.instance.SaveHp(GameManager.instance.PlayerControllerInstance._hp, GameManager.instance.PlayerControllerInstance._maxHp);

        if (victory)
            GameManager.instance.IncreaseSceneIndex(_index + 1);
        else
            GameManager.instance.IncreaseSceneIndex(_index);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(_preSceneName);
    }

    public void Victory()
    {
        SaveValue(true);

        CallScene(_nextSceneName);
    }

    public void Retry()
    {
        SaveValue(false);

        SceneManager.LoadScene(_preSceneName);
    }

    void CallScene(string sceneName)
    {
        LoadingSceneController.LoadScene(sceneName);
    }

    public void EndGame()
    {
        if (!_end)
        {
            _end = true;
            GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(0);
            GameManager.instance.ChangeBGM("end");
        }
    }

    public void StopGame()
    {
        _stop = true;
    }
}
