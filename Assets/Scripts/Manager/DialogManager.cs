using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public bool[] condition;
    public PlayerController playerController;

    [Header("Dialog")]
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _dialog;
    [SerializeField] private GameObject _result;
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _arrow;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Text _typingText;
    [SerializeField] private float _typingSpeed;
    [SerializeField] private AudioClip _messageClip;
    [SerializeField, TextArea] private string[] _script;

    bool[] _printed = new bool[10];

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        NextScript();
    }

    public void Checkcondition()
    {
        if (condition[0])
        {
            OnDialog(0, true, "달이", 1);
            condition[0] = false;
        }

        if (condition[1])
        {
            OnDialog(1, true, "달이", 2);
            condition[1] = false;
        }

        if (condition[2])
        {
            OnDialog(2, true, "달이", 0);
            condition[2] = false;
        }
    }

    private void NextScript()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_printed[0]) // 0 = end
            {
                _printed[0] = false;
                _dialog.SetActive(false);
                _result.SetActive(true);
                Invoke("ControllerOn", 0.5f);
            }

            if (_printed[1])
            {
                _printed[1] = false;
                condition[1] = true;
                Checkcondition();
            }

            if (_printed[2])
            {
                _printed[2] = false;
                condition[2] = true;
                Checkcondition();
            }
        }
    }

    private void OnDialog(int scriptNumber, bool portrait, string name, int nextNumber = 1)
    {
        if (_dialog.activeSelf == false)
        {
            _hud.SetActive(false);
            _dialog.SetActive(true);
        }

        _arrow.enabled = false;

        if (portrait)
        {
            _portrait.enabled = true;
        }
        else
        {
            _portrait.enabled = false;
        }

        _name.text = name;

        StartCoroutine(TypingText(_script[scriptNumber], _typingSpeed, nextNumber));
    }

    IEnumerator TypingText(string message, float speed, int nextNumber)
    {
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
        _printed[nextNumber] = true;
    }
}
