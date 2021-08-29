using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public enum ObstacleType
    {
        None,
        thron,
    }

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage; 
    [SerializeField] private ObstacleType type;

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
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            switch (type)
            {
                case ObstacleType.thron: pc.Damaged(_damage, "thorn"); break;
            }
        }
    }
}
