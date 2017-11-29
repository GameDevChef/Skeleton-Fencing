using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SelectionItem
{
    public RectTransform seletionPartTransform;
    public int selectionPartIndex;
}

public class PlayerUIController : MonoBehaviour {

    CharacterManager m_characterManager;
    PlayerController m_playerController;

    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;
    public bool Apply;

    public GameObject LeftArrow;
    public GameObject RightArrow;
    public GameObject Buttons;
    public GameObject Readytext;


    int m_hatIndex;
    int m_bodyIndex;
    int m_waeponIndex;
    int m_extrasIndex;

    public List<SelectionItem> seletionList;
    private int m_selectionIndex;

    public RectTransform SelectorTransform;

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_characterManager = GetComponent<CharacterManager>();
    } 

    private void Start()
    {
        SelectorTransform.anchoredPosition = seletionList[m_selectionIndex].seletionPartTransform.anchoredPosition;
    }

    public void FixedTick()
    {
        if (m_playerController.IsReady)
        {
            return;
        }
        if (Up)
        {
            
            MoveSelector(true);
        }

        if (Down)
        {
            MoveSelector(false);
        }

        if (Right)
        {
            ChangeSelection(true);
        }

        if (Left)
        {
            ChangeSelection(false);
        }

        if (Apply)
        {
            ApplySelection();
        }
    }

    private void ApplySelection()
    {
        if (m_selectionIndex != seletionList.Count - 1)
            return;
        Buttons.SetActive(false);
        Readytext.SetActive(true);
        m_playerController.IsReady = true;
        GameManager.Instance.Play();
    }

    private void ChangeSelection(bool _right)
    {
        if (m_selectionIndex == seletionList.Count - 1)
            return;

        ITEM_TYPE itemType;
        switch (m_selectionIndex)
        {
            case 0:
                itemType = ITEM_TYPE.HAT;
                break;
            case 1:
                itemType = ITEM_TYPE.BODY;
                break;
            case 2:
                itemType = ITEM_TYPE.WEAPON;
                break;
            default:
                itemType = ITEM_TYPE.HAT;
                break;
        }
    
        m_characterManager.ChangePart(itemType, ref seletionList[m_selectionIndex].selectionPartIndex, _right);


    }

    private void MoveSelector(bool _up)
    {
        m_selectionIndex += (_up) ? -1 : 1;
        if (m_selectionIndex >= seletionList.Count)
            m_selectionIndex = 0;
        else if (m_selectionIndex <= -1)
            m_selectionIndex = seletionList.Count - 1;

        SelectorTransform.anchoredPosition = seletionList[m_selectionIndex].seletionPartTransform.anchoredPosition;
        if(m_selectionIndex == seletionList.Count - 1)
        {
            LeftArrow.SetActive(false);
            RightArrow.SetActive(false);           
        }
        else
        {
            LeftArrow.SetActive(true);
            RightArrow.SetActive(true);
        }
    }
}
