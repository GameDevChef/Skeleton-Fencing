using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterItem
{
    public CharacterPartSprite[] item;
}

[System.Serializable]
public class CharacterPartSprite
{
    public HumanBodyBones bone;
    public Sprite sprite;
}

[System.Serializable]
public class CharacterPartRenderer
{
    public HumanBodyBones bone;
    public SpriteRenderer spriteRenderer;
}

public enum ITEM_TYPE
{
    HAT,
    WEAPON,
    BODY,
    EXTRAS
}


public class CharacterManager : MonoBehaviour {

    public CharacterPartRenderer[] bodyPartsRenderers;
    public CharacterPartRenderer[] hatSpriteRenderers;
    public CharacterPartRenderer[] weaponSpriteRenderers;
    public CharacterPartRenderer[] extrasSpriteRenderers;


    private CharactersItemsSO m_charactersSO;

    private void Awake()
    {
        m_charactersSO = Resources.Load<CharactersItemsSO>("CharactersItemsSO");
    }

    public void ChangePart(ITEM_TYPE _itemType, ref int _index, bool _right)
    {
        CharacterPartRenderer[] rendererArray = GetRenderers(_itemType);
        CharacterPartSprite[] spriteArray = m_charactersSO.GetCharacterPartsList(_itemType, ref _index, _right).item;
        for (int i = 0; i < rendererArray.Length; i++)
        {
            Sprite sprite = GetSprite(rendererArray[i].bone, spriteArray);
            rendererArray[i].spriteRenderer.sprite = sprite;
        }
    }

    Sprite GetSprite(HumanBodyBones bone, CharacterPartSprite[] _characterPartSprites)
    {
        for (int i = 0; i < _characterPartSprites.Length; i++)
        {
            if (_characterPartSprites[i].bone == bone)
            {
                return _characterPartSprites[i].sprite;
            }
        }
        return null;
    }

    CharacterPartRenderer[] GetRenderers(ITEM_TYPE _itemType)
    {
        CharacterPartRenderer[] array = null;
        switch (_itemType)
        {
            case ITEM_TYPE.HAT:
                array = hatSpriteRenderers;
                break;
            case ITEM_TYPE.WEAPON:
                array = weaponSpriteRenderers;
                break;
            case ITEM_TYPE.BODY:;
                array = bodyPartsRenderers;
                break;
            case ITEM_TYPE.EXTRAS:
                array = extrasSpriteRenderers;
                break;
            default:
                break;
        }
        return array;
    }

   

}
