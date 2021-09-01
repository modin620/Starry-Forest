using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    [SerializeField] int _score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            GameManager.totalScore += _score;
            GameManager.instance.PlayScoreTextEffect();
            pc.TakeItem();
            Destroy(gameObject);
        }
    }
}
