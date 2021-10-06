using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningBar : MonoBehaviour
{
    [Header("Running Bar")]
    [SerializeField] Slider _runningBar;
    [SerializeField] Image _fillImage;
    [SerializeField, Range(1, 7)] int _grade = 7;
    [SerializeField] float[] _speedValues = new float[7];
    float _runningGague = 145f;
    bool[] _changedColor = new bool[7];

    [Header("Bar Colors")]
    [SerializeField] Color32[] _nextColors = new Color32[7];

    void Update()
    {
        IncreaseGaugeValue();
        CheckRunningBar();
    }

    void IncreaseGaugeValue()
    {
        _runningGague += Time.deltaTime;
    }

    void CheckRunningBar()
    {
        _runningBar.value = _runningGague;

        for (int i = 0; i < _grade; i++)
        {
            if (_changedColor[i])
                continue;

            if (_runningBar.maxValue / (_grade + 1) * (_grade - i) <= _runningBar.value)
            {
                _changedColor[i] = true;
                ChangeBarColor(_nextColors[i]);
                SpeedUpFloor(_speedValues[_speedValues.Length - (i + 1)]);
            }
        }

        if (_runningGague >= _runningBar.maxValue)
        {
            _runningGague = _runningBar.maxValue;

             if (!GameManager.instance.StageManagerInstance.stop)
                GameManager.instance.StageManagerInstance.StopGame();
        } 
    }

    void ChangeBarColor(Color32 nextColor)
    {
        _fillImage.color = nextColor;
    }

    void SpeedUpFloor(float value)
    {
        GameManager.instance.FloorManagerInstance.SetMoveValue(value);
    }

    public float GetBarPreValue()
    {
        return _runningBar.value;
    }

    public float GetBarMaxValue()
    {
        return _runningBar.maxValue;
    }
}
