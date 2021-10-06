using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _arrow;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Text _typingText;
    [SerializeField] private float _typingSpeed;
    [SerializeField] private AudioClip _messageClip;
    [SerializeField, TextArea] private string[] _script;

    bool[] _condition = new bool[10];
    int _nowScriptNumber = 0;
    int _endScriptNumber = 0;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        NextScript();
    }

    public void SetCondition(int index)
    {
        if (!_condition[index])
        {
            _condition[index] = true;
            Checkcondition();
        }
    }

    public void Checkcondition()
    {
        if (_condition[0])
        {
            PrintScript(0, 2, true, "´ÞÀÌ");
            _condition[0] = false;
        }
    }

    private void NextScript()
    {
        if (Input.GetButtonDown("Submit") && _arrow.enabled)
        {
            if (_nowScriptNumber < _endScriptNumber)
            {
                _nowScriptNumber++;
                StartCoroutine(TypingText(_script[_nowScriptNumber], _typingSpeed));
            }
            else
            {
                GameManager.instance.UIManagerInstance.OnResult();
            }
        }
    }

    private void PrintScript(int start, int end, bool portrait, string name)
    {
        _nowScriptNumber = start;
        _endScriptNumber = end;

        GameManager.instance.UIManagerInstance.OnDialog();

        if (portrait)
            _portrait.enabled = true;
        else
            _portrait.enabled = false;

        _name.text = name;

        StartCoroutine(TypingText(_script[_nowScriptNumber], _typingSpeed));
    }

    IEnumerator TypingText(string message, float speed)
    {
        _arrow.enabled = false;

        for (int i = 0; i < message.Length; i++)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = _messageClip;
                audioSource.Play();
            }

            _typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        audioSource.Stop();
        _arrow.enabled = true;
    }
}
