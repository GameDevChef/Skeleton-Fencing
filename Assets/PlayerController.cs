using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class PlayerController : MonoBehaviour {

    Rigidbody2D m_rb2D;
    Vector3 m_targetVelocity;
     public Animator m_anim;
    public IkLimb2D LeftArmIK;
    public bool isLeft;
    public Transform SpawnPosition;

    public float m_horizontal;
    bool m_isMovingLeft;
    bool m_isMovingRight;
    bool m_isMoving;
    public bool m_Jumping;
    public bool m_stanceUp;
    public bool m_stanceDown;
    public bool m_attack;

    public bool AddExtraVelocity;
    public float AttackVelocityAmount;
    public float CollideVelocityAmount;
    public float currentVelocityAmount;

    public int StanceIndex;

    bool m_onGround;
    bool m_prevOnGround;
    bool m_isRunning;
    bool m_dontMove;
    public bool IsAttacking;
    public float LastAttackTime;

    int m_ignoreLayers;

    public float JumpForce;
    public float AirSpeed;
    public float GroundSpeed;
    float m_targetSpeed;
    float m_currentSpeed;

    public bool IsReady;
    public bool IsDead;

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        LastAttackTime = 0f;
        m_ignoreLayers = ~(1 << 8);
    }


    //public void UpdateBools(float _horizontal, bool _jump, bool _stanceUp, bool _stanceDown, bool _attack)
    //{
    //    m_horizontal = _horizontal;
    //    m_isMovingLeft = (_horizontal < .25);
    //    m_isMovingRight = (_horizontal > .25);
    //    m_isMoving = m_isMovingLeft || m_isMovingRight;
    //    m_Jumping = _jump;
    //    m_stanceUp = _stanceUp;
    //    m_stanceDown = _stanceDown;
    //    m_attack = _attack;
    //    m_anim.SetBool("IsMoving_b", m_isMoving);
    //}

    internal void InitGame()
    {
        transform.position = SpawnPosition.position;
        m_rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void UpdateAxis(float _horizontal)
    {
        m_horizontal = _horizontal;
        m_isMovingLeft = (_horizontal < -.4);
        m_isMovingRight = (_horizontal > .4);
        m_isMoving = (m_isMovingLeft || m_isMovingRight) && !m_dontMove;
        m_anim.SetBool("IsMoving_b", m_isMoving);
    }

    public void FixedTick()
    {
        SetStates();
        HandleAirAnimations();
        HandleScale();
        if (!m_dontMove)
        {
            HandleSideMovement();
            HangleJump();
            HandleStance();
            HandleAttack();
           
        }
        HandleExtraForces();
        m_prevOnGround = m_onGround;
    }

    private void HandleExtraForces()
    {
        if (AddExtraVelocity)
        {
            Debug.Log("extra");
            m_targetVelocity.x = (isLeft) ? 1 : -1;
   
            m_targetVelocity.x *= currentVelocityAmount;
            m_rb2D.velocity = m_targetVelocity;
            AddExtraVelocity = false;
        }
    }

    private void HandleAirAnimations()
    {
        if (!m_onGround && m_prevOnGround)
        {
            m_anim.Play("Jump");
        }
        else if (m_onGround && !m_prevOnGround)
        {
            m_anim.Play("Land");
        }
    }

    void SetStates()
    {
        m_isRunning = m_anim.GetBool("IsRunning_b");
        m_dontMove = m_anim.GetBool("DontMove_b");
        IsAttacking = m_anim.GetBool("Attacking_b");
        m_onGround = CheckGround();
    }

    private void HandleScale()
    {
        Vector3 targetEuler = Vector3.zero;
        if (!isLeft)
        {

            targetEuler.y = 180f;
        }
      
        if (m_isRunning || !m_onGround)
        {
            LeftArmIK.flip = false;
            if (isLeft)
            {
                if (m_horizontal >= 0)
                    targetEuler.y = 0;
                else if (m_horizontal < 0)
                    targetEuler.y = 180;

            }
            else
            {
                if (m_horizontal <= 0)
                    targetEuler.y = 180;
                else if (m_horizontal > 0)
                    targetEuler.y = 0;
            }
        }
        else
        {

            LeftArmIK.flip = true;
        }
        transform.localEulerAngles = targetEuler;
    }

    private void HandleAttack()
    {
        if (!m_onGround)
            return;
        if (m_attack)
        {
            LastAttackTime = Time.time;
            string attackName = "Attack " + StanceIndex;
            m_anim.Play(attackName);
            currentVelocityAmount = AttackVelocityAmount;
            AddExtraVelocity = true;
        }
        
    }

    private void HandleSideMovement()
    {
        
        m_targetVelocity = m_rb2D.velocity;

        if (m_horizontal != 0)
        {
            m_targetVelocity.x = (m_horizontal > 0) ? 1 : -1;
        }
        else
        {
            m_targetVelocity.x = 0f;
        }
       

        m_targetSpeed = (m_onGround) ? GroundSpeed : AirSpeed;
        m_currentSpeed = Mathf.Abs(m_horizontal) * m_targetSpeed;
        m_targetVelocity.x *= m_currentSpeed;

        
        m_rb2D.velocity = m_targetVelocity;
        
    }

    private void HangleJump()
    {
        if (!m_onGround)
        {
            return;
        }

        if (m_Jumping)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        m_rb2D.velocity += Vector2.up * JumpForce; 
    }

    private void HandleStance()
    {
        if (m_stanceUp)
        {
            ChangeStance(1);
        }
        else if (m_stanceDown)
        {
            ChangeStance(-1);
        }
    }

    private void ChangeStance(int _number)
    {
        _number = Mathf.Clamp(_number, -1, 1);
        StanceIndex += _number;
        StanceIndex = Mathf.Clamp(StanceIndex, -1, 1);
        m_anim.SetFloat("Stance_f", StanceIndex);
    }

    bool CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, .5f, m_ignoreLayers))
        {
            return true;
        }
        return false;
    }
}
