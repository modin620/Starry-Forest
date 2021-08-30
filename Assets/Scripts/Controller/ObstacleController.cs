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

    [SerializeField] float _damage; 
    [SerializeField] ObstacleType type;

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
