using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string _nextScene;

    Slider _loadingBar;

    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private void Awake()
    {
        _loadingBar = GetComponent<Slider>();
        _loadingBar.maxValue = 1.0f;
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        const float LOAD_VALUE = 0.2f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < LOAD_VALUE)
            {
                _loadingBar.value = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _loadingBar.value = Mathf.Lerp(LOAD_VALUE, 1.0f, timer);
                
                if (_loadingBar.value >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
