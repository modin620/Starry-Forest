using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    private void Start()
    {
        Invoke("GoTitle", 5f);
    }

    void GoTitle()
    {
        LoadingSceneController.LoadScene("Title");
    }
}
