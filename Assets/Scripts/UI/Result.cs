using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    enum ClearGrade
    {
        Perfect,
        Excellent, 
        Great,
        Good,
        Normal,
        Bad
    }

    [SerializeField] Text _scoreText;
    [SerializeField] Text _clearTimeText;
    [SerializeField] Text _gradeText;
    [SerializeField] float _typingSpeed = 0.025f;

    [SerializeField] float[] _clearTimeTable_1;
    [SerializeField] int _minimuScoreValue_1;

    ClearGrade _myGrade;

    AudioSource audioSource;

    bool _printedScore;
    bool _printedTime;
    bool _printedGrade;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialog(int index)
    {
        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(index);
    }

    public void SetResult()
    {
        PlayerController tempLogic = GameManager.instance.PlayerControllerInstance;

        int hp = tempLogic._hp;
        int maxHp = tempLogic._maxHp;

        int score = GameManager.instance.UIManagerInstance.ScoreInstance.totalScore;

        float clearTime = GameManager.instance.StageManagerInstance.clearTime;

        SetScore(score);

        SetClearTime(clearTime);

        CalcGrade(score, clearTime, hp, maxHp);
    }

    void SetScore(float score)
    {
        _scoreText.text = score.ToString();
    }

    void SetClearTime(float time)
    {
        int minute = 0;
        int second = 0;

        while (time > 60)
        {
            minute++;
            time -= 60;
        }

        second = (int)time;

        string timeStr = string.Format("{0:00}:{1:00}", minute, second);

        _clearTimeText.text = timeStr;
    }

    void CalcGrade(int score, float clearTime, int hp, int maxHp)
    {
        int totalWeight = 0;

        totalWeight += SetScoreWeight(score);
        totalWeight += SetClearTimeWeight(clearTime);
        totalWeight += SetHpWeight(hp, maxHp);

        switch (totalWeight)
        {
            case 30:
            case 29:
            case 28:
                _myGrade = ClearGrade.Perfect;
                break;
            case 27:
            case 26:
            case 25:
            case 24:
                _myGrade = ClearGrade.Excellent;
                break;
            case 23:
            case 22:
            case 21:
            case 20:
                _myGrade = ClearGrade.Great;
                break;
            case 19:
            case 18:
            case 17:
            case 16:
                _myGrade = ClearGrade.Good;
                break;
            case 15:
            case 14:
            case 13:
            case 12:
                _myGrade = ClearGrade.Normal;
                break;
            default:
                _myGrade = ClearGrade.Bad;
                break;
        }

        SetGrade(_myGrade);
    }

    int SetScoreWeight(int score)
    {
        int scoreWeight = 10;

        if (score > _minimuScoreValue_1)
            scoreWeight = 10;
        else
            scoreWeight = 9;

        return scoreWeight;
    }

    int SetClearTimeWeight(float clearTime)
    {
        int clearTimeWeight = 0;

        if (clearTime < _clearTimeTable_1[0])
        {
            clearTimeWeight = 10;
        }
        else if (clearTime < _clearTimeTable_1[1])
        {
            clearTimeWeight = 8;
        }
        else if (clearTime < _clearTimeTable_1[2])
        {
            clearTimeWeight = 6;
        }
        else if (clearTime < _clearTimeTable_1[3])
        {
            clearTimeWeight = 4;
        }
        else if (clearTime < _clearTimeTable_1[4])
        {
            clearTimeWeight = 2;
        }
        else
        {
            clearTimeWeight = 0;
        }

        return clearTimeWeight;
    }

    int SetHpWeight(int hp, int maxHp)
    {
        int scoreWeight = 0;

        if (maxHp == hp)
        {
            scoreWeight = 10;
        }
        else if (maxHp / 2 <= hp)
        {
            scoreWeight = 8;
        }
        else
        {
            scoreWeight = 6;
        }

        return scoreWeight;
    }

    void SetGrade(ClearGrade grade)
    {
        string gradeStr = null;

        switch (grade)
        {
            case ClearGrade.Perfect:
                gradeStr = "참 잘했어요!";
                break;
            case ClearGrade.Excellent:
                gradeStr = "멋져요!";
                break;
            case ClearGrade.Great:
                gradeStr = "잘했어요";
                break;
            case ClearGrade.Good:
                gradeStr = "좋아요";
                break;
            case ClearGrade.Normal:
                gradeStr = "더 열심히";
                break;
            case ClearGrade.Bad:
                gradeStr = "노력하세요!";
                break;
        }

        _gradeText.text = gradeStr;
    }
}
