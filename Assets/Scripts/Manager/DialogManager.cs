using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public bool[] Condition;

    public PlayerController playerController;

    [SerializeField] private GameObject _dialog;
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _arrow;
    [SerializeField] private Text _name;
    [SerializeField] private Text _typingText;
    [SerializeField] private float _typingSpeed;
    [SerializeField] private AudioClip _messageClip;
    [SerializeField, TextArea] private string[] _script;

    public bool[] _printed = new bool[10];

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        NextScript();
    }

    public void CheckCondition()
    {
        if (Condition[0])
        {
            OnDialog(0, true, "달이", 1);
            Condition[0] = false;
        }

        if (Condition[1])
        {
            OnDialog(1, true, "달이");
            Condition[1] = false;
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
                Invoke("ControllerOn", 0.5f);
            }

            if (_printed[1])
            {
                _printed[1] = false;
                Condition[1] = true;
                CheckCondition();
            }
        }
    }

    private void OnDialog(int scriptNumber, bool portrait, string name, int nextNumber = 0)
    {
        if (_dialog.activeSelf == false)
            _dialog.SetActive(true);

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
