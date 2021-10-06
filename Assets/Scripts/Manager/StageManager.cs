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
        SaveValue(false);

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
        }
    }

    public void StopGame()
    {
        _stop = true;
    }

    //public static StageManager instance;

    //public GameObject bloodEffect;
    //public int totalScore;
    //public float progressValue { get { return _progressBar.value; } }
    //public float progressMaxValue { get { return _progressBar.maxValue; } }

    //static Animator scoreTextAnim;

    //public DialogManager dialogManager { get { return dm; } }
    //public FloorManager floorManager { get { return fm; } }
    //public PlayerController playerController { get { return pc; } }

    //PlayerController pc;
    //FloorManager fm;
    //DialogManager dm;

    //[Header("[ UI ]")]
    //[SerializeField] Slider _progressBar;
    //[SerializeField] Image _fillImage;
    //[SerializeField] Image[] _heartImage;
    //[SerializeField] GameObject _scoreBox;
    //[SerializeField] TextMeshProUGUI _scoreText;

    //[SerializeField] float[] _speedArray;
    //[SerializeField] int _nowStageIndex;

    //bool _onEnd;

    //private void Awake()
    //{
    //    if (instance == null)
    //        instance = this;
    //    else
    //        Destroy(gameObject);
    //}

    //private void Start()
    //{
    //    scoreTextAnim = _scoreText.GetComponent<Animator>();
    //    pc = FindObjectOfType<PlayerController>();
    //    fm = FindObjectOfType<FloorManager>();
    //    dm = FindObjectOfType<DialogManager>();
    //}

    //private void Update()
    //{
    //    ScoreUpdate();
    //    HeartUpdate();
    //    Progress();
    //    EndDialog();
    //}

    //public void PlayScoreTextEffect()
    //{
    //    scoreTextAnim.SetTrigger("onTake");
    //}

    //public void PlayBloodEffect()
    //{
    //    bloodEffect.SetActive(true);
    //    Invoke("OffBloodEffect", 0.5f);
    //}

    //public void ReTry()
    //{
    //    CallScene();
    //}

    //public void NextGame()
    //{
    //    _nowStageIndex++;
    //    CallScene();
    //}

    //public void GameOver()
    //{
    //    CallScene();
    //}

    //private void CallScene()
    //{
    //    LoadingSceneController.LoadScene("StageScene_" + _nowStageIndex);
    //}

    //private void OffBloodEffect()
    //{
    //    bloodEffect.SetActive(false);
    //}

    //private void ScoreUpdate()
    //{
    //    RectTransform scoreBoxRect;
    //    scoreBoxRect = _scoreBox.GetComponent<RectTransform>();

    //    if (totalScore >= 100)
    //        scoreBoxRect.anchoredPosition = new Vector3(0, 0, 0);
    //    else if (totalScore >= 10)
    //        scoreBoxRect.anchoredPosition = new Vector3(68, 0, 0);
    //    else
    //        scoreBoxRect.anchoredPosition = new Vector3(113, 0, 0);

    //    _scoreText.text = totalScore.ToString();
    //}

    //private void HeartUpdate()
    //{
    //    switch (pc.Hp)
    //    {
    //        case 4:
    //            for (int i = 0; i < _heartImage.Length; i++)
    //                _heartImage[i].fillAmount = 1.0f;
    //            break;
    //        case 3:
    //            _heartImage[1].fillAmount = 0.5f;
    //            break;
    //        case 2:
    //            _heartImage[1].fillAmount = 0.0f;
    //            break;
    //        case 1:
    //            _heartImage[1].fillAmount = 0.0f;
    //            _heartImage[0].fillAmount = 0.5f;
    //            break;
    //        case 0:
    //            _heartImage[1].fillAmount = 0.0f;
    //            _heartImage[1].fillAmount = 0.0f;
    //            break;
    //    }
    //}

    //private void Progress()
    //{
    //    _progressBar.value += Time.deltaTime;

    //    if (_progressBar.maxValue / 8 * 7 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(178, 63, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 6 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(178, 110, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 5 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(178, 151, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 4 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(167, 178, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 3 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(116, 178, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 2 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(52, 178, 3, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //    else if (_progressBar.maxValue / 8 * 1 <= _progressBar.value)
    //    {
    //        Color32 nextColor = new Color32(3, 178, 33, 255);
    //        _fillImage.color = Color32.Lerp(_fillImage.color, nextColor, 1);
    //    }
    //}

    //private void EndDialog()
    //{
    //    if (!fm.stop)
    //        return;

    //    if (_onEnd)
    //        return;

    //    _onEnd = true;
    //    Invoke("EndDialogRoutine", 1.5f);
    //}

    //private void EndDialogRoutine()
    //{
    //    dm.condition[0] = true;
    //    dm.Checkcondition();
    //}
}
