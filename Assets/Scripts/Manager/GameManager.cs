using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image[] heartImage;
    [SerializeField] private GameObject scoreBox;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerController pc;
    public GameObject bloodEffect;

    private static Animator scoreTextAnim;

    public static int totalScore;

    private void Awake()
    {
        scoreTextAnim = scoreText.GetComponent<Animator>();
    }

    private void Update()
    {
        ScoreUpdate();
        HeartUpdate();
    }

    public static void PlayScoreTextEffect()
    {
        scoreTextAnim.SetTrigger("onTake");
    }

    public void PlayBloodEffect()
    {
        bloodEffect.SetActive(true);
        Invoke("OffBloodEffect", 0.5f);
    }

    private void OffBloodEffect()
    {
        bloodEffect.SetActive(false);
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

    private void HeartUpdate()
    {
        switch (pc.Hp)
        {
            case 2: heartImage[0].fillAmount = 1.0f; break;
            case 1: heartImage[0].fillAmount = 0.5f; break;
            case 0: heartImage[0].fillAmount = 0.0f; break;
        }
    }
}
