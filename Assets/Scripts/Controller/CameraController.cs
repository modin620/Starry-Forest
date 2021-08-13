using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float moveValue;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveVector = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, moveVector, moveValue * Time.deltaTime);
    }
}
