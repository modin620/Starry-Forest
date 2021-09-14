using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDown : MonoBehaviour
{
    [SerializeField] float _shutdownTime;

    private void Start()
    {
        Invoke("FadeDown", _shutdownTime);
    }

    public void FadeDown()
    {
        gameObject.SetActive(false);
    }
}
