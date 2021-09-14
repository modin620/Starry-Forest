using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DialogManager dialogManager { get { return dm; } }
    public GameObject bloodEffect;
    public int totalScore;
    public float progressValue { get { return _progressBar.value; } }
    public float progressMaxValue { get { return _progressBar.maxValue; } }

    static Animator scoreTextAnim;

    [SerializeField] PlayerController pc;
    [SerializeField] FloorManager fc;
    [SerializeField] DialogManager dm;

    [Header("[ UI ]")]
    [SerializeField] Slider _progressBar;
    [SerializeField] Image _fillImage;
    [SerializeField] Image[] _heartImage;
    [SerializeField] GameObject _scoreBox;
    [SerializeField] TextMeshProUGUI _scoreText;

    [SerializeField] float[] _speedArray;
    [SerializeField] int _nowStageIndex;

    bool _onEnd;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        scoreTextAnim = _scoreText.GetComponent<Animator>();
    }

    private void Update()
    {
        ScoreUpdate();
        HeartUpdate();
        Progress();
        EndDialog();
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

    public void ReTry()
    {
        CallScene();
    }

    public void NextGame()
    {
        _nowStageIndex++;
        CallScene();
    }

    public void GameOver()
    {
        CallScene();
    }

    private void CallScene()
    {
        LoadingSceneController.LoadScene("StageScene_" + _nowStageIndex);
    }

    private void OffBloodEffect()
    {
        bloodEffect.SetActive(false);
    }

    private void ScoreUpdate()
    {
        RectTransform scoreBoxRect;
        scoreBoxRect = _scoreBox.GetComponent<RectTransform>();

        if (totalScore >= 100)
            scoreBoxRect.anchoredPosition = new Vector3(0, 0, 0);
        else if (totalScore >= 10)
            scoreBoxRect.anchoredPosition = new Vector3(68, 0, 0);
        else
            scoreBoxRect.anchoredPosition = new Vector3(113, 0, 0);

        _scoreText.text = totalScore.ToString();
    }

    private void HeartUpdate()
    {
        switch (pc.Hp)
        {
            case 4:
                for (int i = 0; i < _heartImage.Length; i++)
                    _heartImage[i].fillAmount = 1.0f;
                break;
            case 3:
                _heartImage[1].fillAmount = 0.5f;
                break;
            case 2:
                _heartImage[1].fillAmount = 0.0f;
                break;
            case 1:
                _heartImage[1].fillAmount = 0.0f;
                _heartImage[0].fillAmount = 0.5f;
                break;
            case 0:
                _heartImage[1].fillAmount = 0.0f;
                _heartImage[1].fillAmount = 0.0f;
                break;
        }
    }

    private void Progress()
    {
        _progressBar.value += Time.deltaTime;

        if (_progressBar.maxValue / 8 * 7 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(178, 63, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 1];
        }
        else if (_progressBar.maxValue / 8 * 6 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(178, 110, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 2];
        }
        else if (_progressBar.maxValue / 8 * 5 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(178, 151, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 3];
        }
        else if (_progressBar.maxValue / 8 * 4 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(167, 178, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 4];
        }
        else if (_progressBar.maxValue / 8 * 3 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(116, 178, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 5];
        }
        else if (_progressBar.maxValue / 8 * 2 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(52, 178, 3, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 6];
        }
        else if (_progressBar.maxValue / 8 * 1 <= _progressBar.value)
        {
            Color32 nextColor = new Color32(3, 178, 33, 255);
            _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
            fc.moveSpeed = _speedArray[_speedArray.Length - 7];
        }
    }

    private void EndDialog()
    {
        if (!FloorManager.stop)
            return;

        if (_onEnd)
            return;

        _onEnd = true;
        Invoke("EndDialogRoutine", 1.5f);
    }

    private void EndDialogRoutine()
    {
        dm.condition[0] = true;
        dm.Checkcondition();
    }
}
