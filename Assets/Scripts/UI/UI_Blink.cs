using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Blink : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _blinkLevel;
    [SerializeField] private float _blinkSpeed;

    private Image _myImage;

    private float _blinkValue;
    private bool _onAdd;

    private void Awake()
    {
        _myImage = GetComponent<Image>();
    }

    private void Update()
    {
        Blink();
        AdjustValue();
    }

    private void Blink()
    {
        _myImage.color = new Color(1, 1, 1, 1 - _blinkValue);

        if (_blinkValue > _blinkLevel)
        {
            _onAdd = false;
        }
    }

    private void AdjustValue()
    {
        if (_onAdd)
        {
            _blinkValue += Time.deltaTime * _blinkSpeed;
        }
        else
        {
            if (_blinkValue < 0)
            {
                _onAdd = true;
            }

            _blinkValue -= Time.deltaTime * _blinkSpeed;
        }
    }
}
