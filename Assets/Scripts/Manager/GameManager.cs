using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject bloodEffect;
    public static int totalScore;

    private static Animator scoreTextAnim;

    [SerializeField] private Image[] heartImage;
    [SerializeField] private GameObject scoreBox;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerController pc;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        scoreTextAnim = scoreText.GetComponent<Animator>();
    }

    private void Update()
    {
        ScoreUpdate();
        HeartUpdate();
    }

    public void PlayScoreTextEffect()
    {
        scoreTextAnim.SetTrigger("onTake");
    }

    public void PlayBloodEffect()
    {
        bloodEffect.SetActive(true);
        Invoke("OffBloodEffect", 0.5f);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("StageScene");
        Debug.Log("GameOver!");
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
            case 4:
                for (int i = 0; i < heartImage.Length; i++)
                    heartImage[i].fillAmount = 1.0f;
                break;
            case 3:
                heartImage[1].fillAmount = 0.5f;
                break;
            case 2:
                heartImage[1].fillAmount = 0.0f;
                break;
            case 1:
                heartImage[1].fillAmount = 0.0f;
                heartImage[0].fillAmount = 0.5f;
                break;
            case 0:
                heartImage[1].fillAmount = 0.0f;
                heartImage[1].fillAmount = 0.0f;
                break;
        }
    }
}
