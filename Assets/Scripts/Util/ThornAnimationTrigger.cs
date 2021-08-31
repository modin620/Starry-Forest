using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornAnimationTrigger : MonoBehaviour
{
    [SerializeField] ObstacleController oc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        oc.onDown = true;
    }
}
