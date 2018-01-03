using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    PlayerController m_playerController;

    [SerializeField]
    PlayerUIController m_playerUIcontroller;

    GameManager m_gameManager;

    [Header("Variables")]

    [SerializeField]
    string m_upButtonName;

    [SerializeField]
    string m_downButtonName;

    [SerializeField]
    string m_applyButtonName;

    [SerializeField]
    string m_jumpButtonName;

    [SerializeField]
    string m_attackButtonName;

    [SerializeField]
    string m_xAxisName;

    [SerializeField]
    string m_yAxisName;

    [SerializeField]
    string m_playerName;

    //Gameplay bools
    float horizontal;
    float vertical;
    bool m_jump;
    bool m_stanceUp;
    bool m_stanceDown;
    bool m_attack;
    bool m_left;
    bool m_right;

    bool m_wasJump;
    bool m_wasStanceUp;
    bool m_wasStanceDown;
    bool m_wasAttack;
    bool m_wasLeft;
    bool m_wasRight;

    //Menu bools
    bool m_upMenu;
    bool m_downmenu;
    bool m_leftMenu;
    bool m_rightMenu;
    bool m_apply;

    bool m_wasUpManu;
    bool m_wasDownMenu;
    bool m_wasLeftMenu;
    bool m_wasRightMenu;
    bool m_wasApply;

    void Start()
    {
        m_gameManager = GameManager.Instance;
    }

    public string CombineInputAndPlayerName(string _input)
    {
        return _input + m_playerName;
    }

    void FixedUpdate()
    {
        switch (m_gameManager.m_GameState)
        {
            case GAME_STATE.MENU:
                GetMenuInput();
                UpdateMenuBools();
                m_playerUIcontroller.MenuFixedTick();
                break;
            case GAME_STATE.GAMEPLAY:
                GetGameplayInput();
                UpdateGameplayBools();
                m_playerController.FixedTick();
                break;
            case GAME_STATE.RESET:
                GetMenuInput();
                UpdateMenuBools();
                m_playerUIcontroller.WinFixedTick();
                break;
            default:
                break;
        }
    }

    void UpdateGameplayBools()
    {
        UpdateBool(ref m_right, ref m_wasRight, ref m_playerController.m_ClickedRight);
        UpdateBool(ref m_left, ref m_wasLeft, ref m_playerController.m_ClickedLeft);
        UpdateBool(ref m_attack, ref m_wasAttack, ref m_playerController.m_Attack);
        UpdateBool(ref m_stanceUp, ref m_wasStanceUp, ref m_playerController.m_StanceUp);
        UpdateBool(ref m_stanceDown, ref m_wasStanceDown, ref m_playerController.m_StandeDown);
        UpdateBool(ref m_jump, ref m_wasJump, ref m_playerController.m_Jumping);
    }

    void GetGameplayInput()
    {
        horizontal = Input.GetAxis(CombineInputAndPlayerName(m_xAxisName));
        m_jump = Input.GetButton(CombineInputAndPlayerName(m_jumpButtonName));
        m_stanceUp = Input.GetButton(CombineInputAndPlayerName(m_upButtonName));
        m_stanceDown = Input.GetButton(CombineInputAndPlayerName(m_downButtonName));
        //m_left = Input.GetButton(CombineInputAndPlayerName(LeftButtonName));
        //m_right = Input.GetButton(CombineInputAndPlayerName(RightButtonName));
        m_left = horizontal < 0f;
        m_right = horizontal > 0f;
        m_attack = Input.GetButton(CombineInputAndPlayerName(m_attackButtonName));
        m_playerController.UpdateAxis(horizontal);
    }

    void UpdateMenuBools()
    {
        UpdateBool(ref m_upMenu, ref m_wasUpManu, ref m_playerUIcontroller.m_Up);
        UpdateBool(ref m_downmenu, ref m_wasDownMenu, ref m_playerUIcontroller.m_Down);
        UpdateBool(ref m_leftMenu, ref m_wasLeftMenu, ref m_playerUIcontroller.m_Left);
        UpdateBool(ref m_rightMenu, ref m_wasRightMenu, ref m_playerUIcontroller.m_Right);
        UpdateBool(ref m_apply, ref m_wasApply, ref m_playerUIcontroller.m_Apply);
    }

    void GetMenuInput()
    {
        horizontal = Input.GetAxis(CombineInputAndPlayerName(m_xAxisName));
        vertical = Input.GetAxis(CombineInputAndPlayerName(m_yAxisName));
        //m_upMenu = Input.GetButton(CombineInputAndPlayerName(UpButtonName));
        //m_downmenu = Input.GetButton(CombineInputAndPlayerName(DownButtonName));
        //m_leftMenu = Input.GetButton(CombineInputAndPlayerName(LeftButtonName));
        //m_rightMenu = Input.GetButton(CombineInputAndPlayerName(RightButtonName));
        m_upMenu = vertical < 0f;
        m_downmenu = vertical > 0f;
        m_leftMenu = horizontal < 0f;
        m_rightMenu = horizontal > 0f;
        m_apply = Input.GetButton(CombineInputAndPlayerName(m_attackButtonName));
    }

    void UpdateBool(ref bool _isBool, ref bool _wasBool, ref bool _stateBool)
    {
        if (_wasBool == false)
        {
            if(_isBool == true)
            {
                _wasBool = true;
                _stateBool = true;
               
            }
        }
        else
        {
            if(_isBool == false)
            {
                _wasBool = false;
                _stateBool = false;
            }
            else
            {
                _stateBool = false;
            }
        }
    }
}
