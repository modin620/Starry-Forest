using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private int _index;
    private bool _nestedCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (_index)
            {
                case 0:
                    if (!_nestedCheck)
                    {
                        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(0);
                        _nestedCheck = true;
                    }
                    break;
            }
        }
    }
}
