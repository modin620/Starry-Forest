using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverItemController : MonoBehaviour
{
    [SerializeField] float _recoverValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = null;
        if (collision.gameObject.tag == "Player")
        {
            pc = collision.GetComponent<PlayerController>();
            pc.Recover(_recoverValue);
            Destroy(gameObject);
        }
    }
}
