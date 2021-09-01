using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornAnimationTrigger : MonoBehaviour
{
    [SerializeField] ObstacleController oc;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        oc.onDown = true;
        audioSource.Play();
    }
}
