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

    [Header("References")]

    [SerializeField]
    List<SelectionItem> m_selectionList;

    [SerializeField]
    RectTransform m_selector;

    [SerializeField]
    GameObject m_leftArrow;

    [SerializeField]
    GameObject m_rightArrow;

    [SerializeField]
    GameObject m_buttons;

    [SerializeField]
    GameObject m_readyText;

    CharacterManager m_characterManager;

    PlayerController m_playerController;

    [Header("Variables")]

    [HideInInspector]
    public bool m_Up;

    [HideInInspector]
    public bool m_Down;

    [HideInInspector]
    public bool m_Left;

    [HideInInspector]
    public bool m_Right;

    [HideInInspector]
    public bool m_Apply;

    int m_hatIndex;

    int m_bodyIndex;

    int m_waeponIndex;

    int m_extrasIndex;

    int m_selectionIndex;


    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_playerController = GetComponent<PlayerController>();
        m_characterManager = GetComponent<CharacterManager>();
    }

    void Start()
    {
        m_selector.anchoredPosition = m_selectionList[m_selectionIndex].seletionPartTransform.anchoredPosition;
    }

    public void MenuFixedTick()
    {
        if (m_playerController.m_IsReady && GameManager.Instance.m_GameState == GAME_STATE.MENU)
        {
            return;
        }
        if (m_Up)
        {          
            MoveSelector(true);
            AudioManager.Instance.PlayMenu("Up Down");
        }

        if (m_Down)
        {
            MoveSelector(false);
            AudioManager.Instance.PlayMenu("Up Down");
        }

        if (m_Right)
        {
            ChangeSelection(true);
            AudioManager.Instance.PlayMenu("Up Down");
        }

        if (m_Left)
        {
            ChangeSelection(false);
            AudioManager.Instance.PlayMenu("Up Down");
        }

        if (m_Apply)
        {
            ApplySelection();          
        }
    }

    public void WinFixedTick()
    {     
        if (m_Apply)
        {
            GameManager.Instance.ResetGame();
            AudioManager.Instance.PlayMenu("Apply");
        }
    }

    void ApplySelection()
    {
        if (m_selectionIndex != m_selectionList.Count - 1)
            return;
        AudioManager.Instance.PlayMenu("Apply");
        m_buttons.SetActive(false);
        m_readyText.SetActive(true);
        m_playerController.m_IsReady = true;
        GameManager.Instance.Play();
    }

    void ChangeSelection(bool _right)
    {
        if (m_selectionIndex == m_selectionList.Count - 1)
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
    
        m_characterManager.ChangePart(itemType, ref m_selectionList[m_selectionIndex].selectionPartIndex, _right);


    }

    void MoveSelector(bool _up)
    {
        m_selectionIndex += (_up) ? -1 : 1;
        if (m_selectionIndex >= m_selectionList.Count)
            m_selectionIndex = 0;
        else if (m_selectionIndex <= -1)
            m_selectionIndex = m_selectionList.Count - 1;

        m_selector.anchoredPosition = m_selectionList[m_selectionIndex].seletionPartTransform.anchoredPosition;
        if(m_selectionIndex == m_selectionList.Count - 1)
        {
            m_leftArrow.SetActive(false);
            m_rightArrow.SetActive(false);           
        }
        else
        {
            m_leftArrow.SetActive(true);
            m_rightArrow.SetActive(true);
        }
    }
}
