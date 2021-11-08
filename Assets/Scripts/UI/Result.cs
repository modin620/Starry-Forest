using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public void StartDialog(int index)
    {
        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(index);
    }
}
