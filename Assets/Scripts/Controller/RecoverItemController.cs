using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverItemController : MonoBehaviour
{
    [SerializeField] int _recoverValue;
    [SerializeField] bool _onItem;

    private void Awake()
    {
        int ranInt = Random.Range(0, 5);
        if (ranInt == 0)
            _onItem = true;
    }

    private void Start()
    {
        if (_onItem)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

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
