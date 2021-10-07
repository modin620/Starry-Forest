using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Mushroom,
    Potion
}

public class ItemInfo
{
    public int _score;
}

public class ItemController : MonoBehaviour
{
    [SerializeField] protected ItemType _myType = ItemType.None;
    [SerializeField] int _mushroomScore = 1;
    [SerializeField] int _potionScore = 1;

    protected ItemInfo _info = new ItemInfo();

    protected void SetInfo()
    {
        switch (_myType)
        {
            case ItemType.Mushroom:
                _info._score = _mushroomScore;
                break;

            case ItemType.Potion:
                _info._score = _potionScore;
                break;
        }
    }
}
