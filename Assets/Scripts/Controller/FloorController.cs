using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public static bool stop;

    public float moveSpeed;

    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] List<GameObject> _preFloor;
    [SerializeField] GameObject _lastFloor;

    bool _onLast;

    private void Update()
    {
        Move();

        if (GameManager.instance.progressValue >= GameManager.instance.progressMaxValue)
            Replace(true);
        else
            Replace(false);
    }

    private void Move()
    {
        if (_onLast && _preFloor[_preFloor.Count - 1].gameObject.transform.position.x <= 1)
        {
            stop = true;
            return;
        }

        Vector2 moveVec = new Vector2(-1, 0);

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                _preFloor[i].transform.Translate(moveVec * moveSpeed * Time.deltaTime);
    }

    private void Replace(bool onLast)
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

    private void CreateFloor(bool onLast)
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

    private void CallLastFloor()
    { 
        if (GameManager.instance.progressValue >= GameManager.instance.progressMaxValue)
        {
            Replace(true);
        }
    }
}
