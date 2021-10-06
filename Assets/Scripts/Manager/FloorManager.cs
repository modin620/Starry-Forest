using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] List<GameObject> _preFloor;
    [SerializeField] GameObject _lastFloor;
    [SerializeField] float _moveSpeed;
    bool _onLast;

    RunningBar runningBar;

    private void Start()
    {
        runningBar = GameManager.instance.UIManagerInstance.runningBarInstance;
    }

    void Update()
    {
        Move();

        if (runningBar.GetBarPreValue() >= runningBar.GetBarMaxValue())
            Replace(true);
        else
            Replace(false);
    }

    void Move()
    {
        if (_onLast && _preFloor[_preFloor.Count - 1].gameObject.transform.position.x <= 0)
        {
            GameManager.instance.StageManagerInstance.EndGame();
            return;
        }

        Vector2 moveVec = Vector2.left;

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                _preFloor[i].transform.Translate(moveVec * _moveSpeed * Time.deltaTime);
    }

    void Replace(bool onLast)
    {
        if (_onLast)
            return;

        const float LIMIT_VALUE = -71.0f;

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                if (_preFloor[i].transform.position.x <= LIMIT_VALUE)
                { 
                    if (onLast)
                    {
                        _onLast = true;
                        Destroy(_preFloor[i].gameObject);
                        _preFloor.RemoveAt(i);
                        CreateFloor(true);
                    }
                    else
                    {
                        Destroy(_preFloor[i].gameObject);
                        _preFloor.RemoveAt(i);
                        CreateFloor(false);
                    }
                }
    }

    void CreateFloor(bool onLast)
    {
        const float REPOSITION_VALUE = 35.5f;
        Vector2 createPoint = new Vector2(REPOSITION_VALUE, transform.position.y);

        if (onLast)
        {
            GameObject lastFloor = Instantiate(_lastFloor, transform);
            _preFloor.Add(lastFloor);
            lastFloor.transform.position = createPoint;
        }
        else
        {
            int floorIndex = Random.Range(0, _floorPrefabs.Length);
            GameObject newFloor = Instantiate(_floorPrefabs[floorIndex], transform);
            _preFloor.Add(newFloor);
            newFloor.transform.position = createPoint;
        }
    }

    void CallLastFloor()
    {
        RunningBar runningBarLogic = GameManager.instance.UIManagerInstance.runningBarInstance;

        if (runningBarLogic.GetBarPreValue() >= runningBarLogic.GetBarMaxValue())
            Replace(true);
    }

    public void SetMoveValue(float value)
    {
        _moveSpeed = value;
    }
}
