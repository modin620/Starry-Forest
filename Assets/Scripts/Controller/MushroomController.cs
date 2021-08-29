using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    [SerializeField] private int _score;
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveVec = new Vector2(-1, 0);
        transform.Translate(moveVec * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            GameManager.totalScore += _score;
            GameManager.PlayScoreTextEffect();
            pc.TakeItem();
            Destroy(gameObject);
        }
    }
}
