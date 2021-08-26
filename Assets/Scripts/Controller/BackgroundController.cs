using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Transform[] _firstLayer;
    [SerializeField] private Transform[] _secondLayer;
    [SerializeField] private Transform[] _thirdLayer;
    [SerializeField] private Transform[] _forthLayer;
    [SerializeField] private Transform[] _obstacle;

    [SerializeField] private float[] _moveSpeed;

    private void Update()
    {
        MoveLayer();
        Reposition();
    }

    private void MoveLayer()
    {
        Vector2 moveVec = new Vector2(-1, 0);

        for (int i = 0; i < _firstLayer.Length; i++)
            _firstLayer[i].Translate(moveVec * _moveSpeed[0] * Time.deltaTime);

        for (int i = 0; i < _secondLayer.Length; i++)
            _secondLayer[i].Translate(moveVec * _moveSpeed[1] * Time.deltaTime);

        for (int i = 0; i < _thirdLayer.Length; i++)
            _thirdLayer[i].Translate(moveVec * _moveSpeed[2] * Time.deltaTime);

        for (int i = 0; i < _forthLayer.Length; i++)
            _forthLayer[i].Translate(moveVec * _moveSpeed[3] * Time.deltaTime);
    }

    private void Reposition()
    {
        const float LIMIT_VALUE = -71.0f;
        const float REPOSITION_VALUE = 35.5f;

        for (int i = 0; i <_firstLayer.Length; i++)
            if (_firstLayer[i].position.x <= LIMIT_VALUE)
                _firstLayer[i].position = new Vector2(REPOSITION_VALUE, 0);

        for (int i = 0; i < _secondLayer.Length; i++)
            if (_secondLayer[i].position.x <= LIMIT_VALUE)
                _secondLayer[i].position = new Vector2(REPOSITION_VALUE, 0);

        for (int i = 0; i < _thirdLayer.Length; i++)
            if (_thirdLayer[i].position.x <= LIMIT_VALUE)
                _thirdLayer[i].position = new Vector2(REPOSITION_VALUE, 0);

        for (int i = 0; i < _forthLayer.Length; i++)
            if (_forthLayer[i].position.x <= LIMIT_VALUE)
                _forthLayer[i].position = new Vector2(REPOSITION_VALUE, 0);
    }
}
