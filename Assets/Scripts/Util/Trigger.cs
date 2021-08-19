using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public DialogManager dialogManager;

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
                        dialogManager.Condition[0] = true;
                        dialogManager.CheckCondition();
                        _nestedCheck = true;
                    }
                    break;
            }
        }
    }
}
