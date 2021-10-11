using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject Dialog;
    [SerializeField] GameObject Result;
    [SerializeField] GameObject Guide;
    [SerializeField] GameObject Option;

    RunningBar _runningBar;
    Heart _heart;
    Score _score;
    Blood _blood;
    Dialog _dialog;
    Option _option;

    public RunningBar runningBarInstance { get { return _runningBar; } }
    public Heart heartInstance { get { return _heart; } }
    public Score ScoreInstance { get { return _score; } }
    public Blood BloodInstance { get { return _blood; } }
    public Dialog DialogInstance { get { return _dialog; } }

    private void Awake()
    {
        _runningBar = GameObject.Find("RunningBox").GetComponent<RunningBar>();
        _heart = GameObject.Find("HeartBox").GetComponent<Heart>();
        _score = GameObject.Find("ScoreBox").GetComponent<Score>();
        _blood = GameObject.Find("BloodBox").GetComponent<Blood>();

        _dialog = Dialog.GetComponent<Dialog>();
        _option = Option.GetComponent<Option>();
    }

    public void OnDialog()
    {
        HUD.SetActive(false);

        Dialog.SetActive(true);
    }

    public void OnResult()
    {
        Dialog.SetActive(false);

        Result.SetActive(true);
    }

    public void OnOption()
    {
        Option.SetActive(true);
        GameManager.instance.GameStop();
    }

    public void OffOption()
    {
        Option.SetActive(false);
        GameManager.instance.GameContinue();
    }
}
