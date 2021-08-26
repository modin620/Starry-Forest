using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreBox;
    [SerializeField] private TextMeshProUGUI scoreText;

    public static int totalScore;

    private void Update()
    {
        ScoreUpdate();
    }

    private void ScoreUpdate()
    {
        RectTransform scoreBoxRect;
        scoreBoxRect = scoreBox.GetComponent<RectTransform>();

        if (totalScore >= 100)
            scoreBoxRect.anchoredPosition = new Vector3(0, 0, 0);
        else if (totalScore >= 10)
            scoreBoxRect.anchoredPosition = new Vector3(68, 0, 0);
        else
            scoreBoxRect.anchoredPosition = new Vector3(113, 0, 0);

        scoreText.text = totalScore.ToString();
    }
}
