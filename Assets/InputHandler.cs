using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public PlayerController PlayerControllerIns;
    public PlayerUIController PlayerUI;
    GameManager m_gameManager;

    public string PlayerName;

    //Gameplay bools
    float horizontal;
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

    //menu bools
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

    public string LeftButtonName;
    public string RightButtonName;
    public string UpButtonName;
    public string DownButtonName;
    public string ApplyButtonName;
    public string JumpButtonName;
    public string AttackButtonName;
    public string AxisName;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
    }

    public string CombineInputAndPlayerName(string _input)
    {
        return _input + PlayerName;
    }

    void FixedUpdate()
    {
        switch (m_gameManager.GameState)
        {
            case GAME_STATE.MENU:
                GetMenuInput();
                UpdateMenuBools();
                PlayerUI.FixedTick();
                break;
            case GAME_STATE.GAMEPLAY:
                GetGameplayInput();
                UpdateGameplayBools();
                PlayerControllerIns.FixedTick();
                break;
            default:
                break;
        }


    }

    void UpdateGameplayBools()
    {
        UpdateBool(ref m_right, ref m_wasRight, ref PlayerControllerIns.ClickedRight);
        UpdateBool(ref m_left, ref m_wasLeft, ref PlayerControllerIns.ClicedLeft);
        UpdateBool(ref m_attack, ref m_wasAttack, ref PlayerControllerIns.m_attack);
        UpdateBool(ref m_stanceUp, ref m_wasStanceUp, ref PlayerControllerIns.m_stanceUp);
        UpdateBool(ref m_stanceDown, ref m_wasStanceDown, ref PlayerControllerIns.m_stanceDown);
        UpdateBool(ref m_jump, ref m_wasJump, ref PlayerControllerIns.m_Jumping);
    }

    void GetGameplayInput()
    {
        horizontal = Input.GetAxis(CombineInputAndPlayerName(AxisName));
        m_jump = Input.GetButton(CombineInputAndPlayerName(JumpButtonName));
        m_stanceUp = Input.GetButton(CombineInputAndPlayerName(UpButtonName));
        m_stanceDown = Input.GetButton(CombineInputAndPlayerName(DownButtonName));
        m_left = Input.GetButton(CombineInputAndPlayerName(LeftButtonName));
        m_right = Input.GetButton(CombineInputAndPlayerName(RightButtonName));
        m_attack = Input.GetButton(CombineInputAndPlayerName(AttackButtonName));
        PlayerControllerIns.UpdateAxis(horizontal);
    }

    void UpdateMenuBools()
    {
        UpdateBool(ref m_upMenu, ref m_wasUpManu, ref PlayerUI.Up);
        UpdateBool(ref m_downmenu, ref m_wasDownMenu, ref PlayerUI.Down);
        UpdateBool(ref m_leftMenu, ref m_wasLeftMenu, ref PlayerUI.Left);
        UpdateBool(ref m_rightMenu, ref m_wasRightMenu, ref PlayerUI.Right);
        UpdateBool(ref m_apply, ref m_wasApply, ref PlayerUI.Apply);
    }

    void GetMenuInput()
    {
        m_upMenu = Input.GetButton(CombineInputAndPlayerName(UpButtonName));
        m_downmenu = Input.GetButton(CombineInputAndPlayerName(DownButtonName));
        m_leftMenu = Input.GetButton(CombineInputAndPlayerName(LeftButtonName));
        m_rightMenu = Input.GetButton(CombineInputAndPlayerName(RightButtonName));
        m_apply = Input.GetButton(CombineInputAndPlayerName(ApplyButtonName));
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
