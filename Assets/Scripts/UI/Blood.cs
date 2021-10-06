using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [Header("Blood")]
    [SerializeField] GameObject _blood;
    [SerializeField] float _playTime = 0.4f;

    public void PlayBlood()
    {
        _blood.SetActive(true);
        Invoke("QuitBlood", _playTime);
    }

    void QuitBlood()
    {
        _blood.SetActive(false);
    }
}
