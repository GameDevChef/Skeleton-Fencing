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

    bool m_wasJump;
    bool m_wasStanceUp;
    bool m_wasStanceDown;
    bool m_wasAttack;

    //menu bools
    bool m_up;
    bool m_down;
    bool m_left;
    bool m_right;
    bool m_apply;

    bool m_wasUp;
    bool m_wasDown;
    bool m_wasLeft;
    bool m_wasRight;
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
        m_attack = Input.GetButton(CombineInputAndPlayerName(AttackButtonName));
        PlayerControllerIns.UpdateAxis(horizontal);
    }

    void UpdateMenuBools()
    {
        UpdateBool(ref m_up, ref m_wasUp, ref PlayerUI.Up);
        UpdateBool(ref m_down, ref m_wasDown, ref PlayerUI.Down);
        UpdateBool(ref m_left, ref m_wasLeft, ref PlayerUI.Left);
        UpdateBool(ref m_right, ref m_wasRight, ref PlayerUI.Right);
        UpdateBool(ref m_apply, ref m_wasApply, ref PlayerUI.Apply);
    }

    void GetMenuInput()
    {       
        m_up = Input.GetButton(CombineInputAndPlayerName(UpButtonName));
        m_down = Input.GetButton(CombineInputAndPlayerName(DownButtonName));
        m_left = Input.GetButton(CombineInputAndPlayerName(LeftButtonName));
        m_right = Input.GetButton(CombineInputAndPlayerName(RightButtonName));
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
