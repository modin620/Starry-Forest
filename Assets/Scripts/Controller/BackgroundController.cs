using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Transform[] _firstLayer;
    [SerializeField] Transform[] _secondLayer;

    [SerializeField] float[] _moveSpeed;

    private void Update()
    {
        MoveLayer();
        Reposition();
    }

    private void MoveLayer()
    {
        if (FloorManager.stop)
            return;

        Vector2 moveVec = new Vector2(-1, 0);

        for (int i = 0; i < _firstLayer.Length; i++)
            _firstLayer[i].Translate(moveVec * _moveSpeed[0] * Time.deltaTime);

        for (int i = 0; i < _secondLayer.Length; i++)
            _secondLayer[i].Translate(moveVec * _moveSpeed[1] * Time.deltaTime);
    }

    private void Reposition()
    {
        if (FloorManager.stop)
            return;

        const float LIMIT_VALUE = -71.0f;
        const float REPOSITION_VALUE = 35.5f;

        for (int i = 0; i <_firstLayer.Length; i++)
            if (_firstLayer[i].position.x <= LIMIT_VALUE)
                _firstLayer[i].position = new Vector2(REPOSITION_VALUE, 0);

        for (int i = 0; i < _secondLayer.Length; i++)
            if (_secondLayer[i].position.x <= LIMIT_VALUE)
                _secondLayer[i].position = new Vector2(REPOSITION_VALUE, 0);
    }
}
