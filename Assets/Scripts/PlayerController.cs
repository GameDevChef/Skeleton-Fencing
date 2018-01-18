using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class PlayerController : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Transform m_spawnTransform;

    [SerializeField]
    public GameObject[] m_iks;

    [SerializeField]
    ParticleSystem m_leftStepEffect;

    [SerializeField]
    ParticleSystem m_rightStepEffect;

    [SerializeField]
    ParticleSystem m_bloodEffect;

    Rigidbody2D m_rb2D;

    Vector3 m_targetVelocity;

    Animator m_anim;

    [Header("Variables")]

    public bool m_IsLeft;

    [SerializeField]
    float m_attackVelocityAmount;

    public float m_ColliderVelocityAmount;

    [SerializeField]
    float m_jumpForce;

    [SerializeField]
    float m_airSpeed;

    [SerializeField]
    float m_groundSpeed;

    [HideInInspector]
    public float currentVelocityAmount;

    [HideInInspector]
    public float m_Horizontal;

    [HideInInspector]
    public bool m_Jumping;

    [HideInInspector]
    public bool m_StanceUp;

    [HideInInspector]
    public bool m_StandeDown;

    [HideInInspector]
    public bool m_Attack;

    [HideInInspector]
    public bool m_ClickedLeft;

    [HideInInspector]
    public bool m_ClickedRight;

    [HideInInspector]
    public bool m_AddExtraVelocity;

    [HideInInspector]
    public int m_StanceIndex = 0;

    //[HideInInspector]
    public bool m_IsReady;

    [HideInInspector]
    public bool m_IsDead;

    [HideInInspector]
    public bool m_HasWon;

    [HideInInspector]
    public bool m_DontMove;

    [HideInInspector]
    public bool m_IsAttacking;

    [HideInInspector]
    public float m_LastAttackTime;

    bool m_canMove;

    bool m_isMovingLeft;

    bool m_isMovingRight;

    bool m_wasMovingLeft;

    bool m_wasMovingRight;

    bool m_isMoving;
    //bool m_onGround;
    //bool m_prevOnGround;
    //bool m_isRunning;
    int m_ignoreLayers;

    float m_targetSpeed;

    float m_currentSpeed;

    float m_animStance;


    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
    }

    public void Reset()
    {      
        SwitchIKs(true);
        m_IsDead = false;
        m_HasWon = false;
        m_LastAttackTime = 0f;
        m_DontMove = false;
        m_IsAttacking = false;
        m_anim.SetBool("IsDead_b", m_IsDead);
        transform.position = m_spawnTransform.position;
        m_canMove = false;
        OnStart();
    }

    void Start()
    {
        m_LastAttackTime = 0f;
        m_ignoreLayers = ~(1 << 8);
    }

    public void OnStart()
    {
        Invoke("StartPlayer", 2f);
    }

    void StartPlayer()
    {
        m_canMove = true;
    }

    void SwitchIKs(bool _on)
    {
        for (int i = 0; i < m_iks.Length; i++)
        {
            m_iks[i].SetActive(_on);
        }
    }

    public void PlayPushBack()
    {
         m_anim.Play("Push back");
    }

    public void InitGame()
    {
        transform.position = m_spawnTransform.position;
        m_rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void UpdateAxis(float _horizontal)
    {
        m_Horizontal = _horizontal;
        m_isMovingLeft = (_horizontal < -.4);
        m_isMovingRight = (_horizontal > .4);
        m_isMoving = (m_isMovingLeft || m_isMovingRight) && !m_DontMove;
        m_anim.SetBool("IsMoving_b", m_isMoving);
        m_wasMovingLeft = m_isMoving;
        m_wasMovingRight = m_isMovingRight;
    }

    void HandleStep()
    {
        if (m_ClickedLeft)
        {           
            ParticleSystem effect = (m_IsLeft) ? m_rightStepEffect : m_rightStepEffect;
            PlayEffect(effect);
            AudioManager.Instance.PlayPlayerFX("step", m_IsLeft);
        }
        if (m_ClickedRight)
        {
            ParticleSystem effect = (m_IsLeft) ? m_leftStepEffect : m_leftStepEffect;
            PlayEffect(effect);
            AudioManager.Instance.PlayPlayerFX("step", m_IsLeft);
        }
       
    }

    void PlayEffect(ParticleSystem _effect)
    {
        _effect.Stop();
        _effect.Play();
    }

    public void FixedTick()
    {
        if (!m_canMove)
        {
            return;
        }
            
        HandleStep();
        SetStates();
       // HandleAirAnimations();
       // HandleScale();
        if (!m_DontMove)
        {
            HandleSideMovement();
            //HangleJump();
            HandleStance();
            HandleAttack();
           
        }
        HandleExtraForces();
        // m_prevOnGround = m_onGround;
        
    }

    void HandleExtraForces()
    {
        if (m_AddExtraVelocity)
        {           
            m_targetVelocity.x = (m_IsLeft) ? 1 : -1;
            m_targetVelocity.x *= currentVelocityAmount;
            m_rb2D.velocity = m_targetVelocity;
            ParticleSystem effect = (m_targetVelocity.x < 0) ? m_rightStepEffect : m_leftStepEffect;
            PlayEffect(effect);
            m_AddExtraVelocity = false;
        }
    }

    public void Die()
    {
        m_IsDead = true;
        m_anim.SetBool("IsDead_b", m_IsDead);
        m_anim.Play("Die");
        PlayEffect(m_bloodEffect);
        AudioManager.Instance.PlayPlayerFX("die", m_IsLeft);
        GameManager.Instance.OnEndTurn();
    }

    public void Win()
    {
        m_HasWon = true;       
        ScoreManager.Instance.AddScore(this);
        GameManager.Instance.OnEndTurn();
    }

    //private void HandleAirAnimations()
    //{
    //    if (!m_onGround && m_prevOnGround)
    //    {
    //        m_anim.Play("Jump");
    //    }
    //    else if (m_onGround && !m_prevOnGround)
    //    {
    //        m_anim.Play("Land");
    //    }
    //}

    void SetStates()
    {
        //m_isRunning = m_anim.GetBool("IsRunning_b");
        m_DontMove = m_anim.GetBool("DontMove_b");
        m_IsAttacking = m_anim.GetBool("Attacking_b");
       // m_onGround = CheckGround();
    }

    //private void HandleScale()
    //{
    //    Vector3 targetEuler = Vector3.zero;
    //    if (!isLeft)
    //    {

    //        targetEuler.y = 180f;
    //    }
      
    //    if (m_isRunning || !m_onGround)
    //    {
    //        LeftArmIK.flip = false;
    //        if (isLeft)
    //        {
    //            if (m_horizontal >= 0)
    //                targetEuler.y = 0;
    //            else if (m_horizontal < 0)
    //                targetEuler.y = 180;

    //        }
    //        else
    //        {
    //            if (m_horizontal <= 0)
    //                targetEuler.y = 180;
    //            else if (m_horizontal > 0)
    //                targetEuler.y = 0;
    //        }
    //    }
    //    else
    //    {

    //        LeftArmIK.flip = true;
    //    }
    //    transform.localEulerAngles = targetEuler;
    //}

    void HandleAttack()
    {
        //if (!m_onGround)
        //    return;
        if (m_Attack)
        {
            AudioManager.Instance.PlayPlayerFX("sword", m_IsLeft);
            m_LastAttackTime = Time.time;
            string attackName = "Attack " + m_StanceIndex;
            m_anim.Play(attackName);
            currentVelocityAmount = m_attackVelocityAmount;
            m_AddExtraVelocity = true;
        }
        
    }

    void HandleSideMovement()
    {       
        m_targetVelocity = m_rb2D.velocity;

        if (m_Horizontal != 0)
        {
            m_targetVelocity.x = (m_Horizontal > 0) ? 1 : -1;
        }
        else
        {
            m_targetVelocity.x = 0f;
        }

        
        //m_targetSpeed = (m_onGround) ? GroundSpeed : AirSpeed;
        m_targetSpeed = m_groundSpeed;
        m_currentSpeed = Mathf.Abs(m_Horizontal) * m_targetSpeed;
        m_targetVelocity.x *= m_currentSpeed;

        //HandleStep();
        m_rb2D.velocity = m_targetVelocity;
        
    }

    //void HangleJump()
    //{
    //    if (!m_onGround)
    //    {
    //        return;
    //    }

    //    if (m_Jumping)
    //    {
    //        PerformJump();
    //    }
    //}

    //private void PerformJump()
    //{
    //    m_rb2D.velocity += Vector2.up * JumpForce; 
    //}

    void HandleStance()
    {
       
        if (m_StanceUp)
        {
            ChangeStance(1);
        }
        else if (m_StandeDown)
        {
            ChangeStance(-1);
        }
        m_animStance = Mathf.MoveTowards(m_animStance, m_StanceIndex, Time.deltaTime * 5f);
        m_anim.SetFloat("Stance_f", m_animStance);
    }

    void ChangeStance(int _number)
    {
        m_animStance = m_StanceIndex;
        _number = Mathf.Clamp(_number, -1, 1);
        m_StanceIndex += _number;
        m_StanceIndex = Mathf.Clamp(m_StanceIndex, -1, 1);
        
    }

    //bool CheckGround()
    //{
    //    if (Physics2D.Raycast(transform.position, Vector2.down, .5f, m_ignoreLayers))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

}
