using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _moveValue;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveVector = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, moveVector, _moveValue * Time.deltaTime);
    }
}
