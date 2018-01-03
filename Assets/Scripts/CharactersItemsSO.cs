using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[CreateAssetMenu]
public class CharactersItemsSO : ScriptableObject
{
    [SerializeField]
    CharacterItem[] hatItems;

    [SerializeField]
    CharacterItem[] bodyItems;

    [SerializeField]
    CharacterItem[] weaponItems;

    public CharacterItem GetCharacterPartsList(ITEM_TYPE _itemType, ref int _index, bool _right)
    {
        CharacterItem[] list = null;
        switch (_itemType)
        {
            case ITEM_TYPE.HAT:
                list = hatItems;
                break;
            case ITEM_TYPE.WEAPON:
                list = weaponItems;
                break;
            case ITEM_TYPE.BODY:
                list = bodyItems;
                break;
            case ITEM_TYPE.EXTRAS:
                break;
            default:
                break;
        }
        _index += (_right) ? 1 : -1;
        
        if (_index == -1)
            _index = list.Length - 1;
        else if (_index >= list.Length)
            _index = 0;
        return list[_index];
    }
}

