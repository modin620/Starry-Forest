using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] List<GameObject> _preFloor;

    private void Update()
    {
        Move();
        Replace();
    }

    private void Move()
    {
        Vector2 moveVec = new Vector2(-1, 0);

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                _preFloor[i].transform.Translate(moveVec * moveSpeed * Time.deltaTime);
    }

    private void Replace()
    {
        const float LIMIT_VALUE = -71.0f;

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                if (_preFloor[i].transform.position.x <= LIMIT_VALUE)
                { 
                    Destroy(_preFloor[i].gameObject);
                    _preFloor.RemoveAt(i);
                    CreateFloor();
                }
    }

    private void CreateFloor()
    {
        const float REPOSITION_VALUE = 35.5f;
        Vector2 createPoint = new Vector2(REPOSITION_VALUE, transform.position.y);

        int floorIndex = Random.Range(0, _floorPrefabs.Length);
        GameObject newFloor = Instantiate(_floorPrefabs[floorIndex], transform);
        _preFloor.Add(newFloor);
        newFloor.transform.position = createPoint;
    }
}
