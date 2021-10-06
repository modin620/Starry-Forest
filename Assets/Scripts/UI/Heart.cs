using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [Header("Heart")]
    [SerializeField] Image[] _heartImages;

    int _hp;
    int _maxHp;

    PlayerController playerController;

    public void CheckHeart()
    {
         _hp = GameManager.instance.PlayerControllerInstance._hp;
        _maxHp = GameManager.instance.PlayerControllerInstance._maxHp;

        for (int i = 0; i < _heartImages.Length; i++)
            _heartImages[i].fillAmount = 1f;

        for (int i = _maxHp; i > _hp; i--)
            _heartImages[(i - 1)].fillAmount = 0f;
    }
}