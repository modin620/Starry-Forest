using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] public GameObject _scoreBox;
    public TextMeshProUGUI _scoreText;
    [SerializeField] Vector3[] _changeVector;
    [SerializeField] Animator _scoreAnim;
    int _totalScore = 0;

    public int totalScore { get { return _totalScore; } set { _totalScore = value; } }

    private void Start()
    {
        _totalScore = GameManager.instance.LoadTotalScore();

        CheckScore();
    }

    public void CheckScore(int score = 0)
    {
        _totalScore += score;

        RectTransform scoreBoxRect = _scoreBox.GetComponent<RectTransform>();

        if (totalScore > 99)
            scoreBoxRect.anchoredPosition = _changeVector[0];
        else if (totalScore > 9)
            scoreBoxRect.anchoredPosition = _changeVector[1];
        else
            scoreBoxRect.anchoredPosition = _changeVector[2];

        _scoreText.text = _totalScore.ToString();

        PlayScoreAnimation();
    }

    public void PlayScoreAnimation()
    {
        _scoreAnim.SetTrigger("onTake");
    }
}
