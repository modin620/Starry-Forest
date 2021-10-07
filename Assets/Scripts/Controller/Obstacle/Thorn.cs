using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : ObstacleController
{
    private void Awake()
    {
        base.SetInfo();

        _myType = ObstacleType.Thron;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            pc.Damaged(_info._damage, "thorn");

            GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();
        }
    }
}
