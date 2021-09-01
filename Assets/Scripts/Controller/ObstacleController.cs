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
    public bool onDown;

    [SerializeField] float _damage;
    [SerializeField] float _downSpeed;
    [SerializeField] ObstacleType type;

    private void Update()
    {
        Down();
    }

    private void Down()
    {
        if (!onDown)
            return;

        Vector2 downVec = new Vector2(transform.position.x, -0.5f);
        transform.position = Vector2.Lerp(transform.position, downVec, _downSpeed * Time.deltaTime);

        if (transform.position.y <= -0.5f)
            onDown = false;
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
