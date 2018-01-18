using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    PlayerController m_player1Controller;

    [SerializeField]
    PlayerController m_player2Controller;

    [SerializeField]
    ParticleSystem m_clashEffect;

    [SerializeField]
    float m_checkingInterval;

    float m_currentWaitTime;

    [Header("Variables")]

    Vector3 m_clashPosition;

    void Update()
    {
        m_currentWaitTime += Time.deltaTime;
    }

    public void HandleCombat(Collider2D _other)
    {
        if (m_currentWaitTime < m_checkingInterval)
            return;          

        if (m_player1Controller.m_IsDead || m_player2Controller.m_IsDead)
            return;

        m_currentWaitTime = 0f;
        

        if (_other.CompareTag("Sword"))
        {
            HandleCollision();
            SetClashPosition(_other.transform.position);
        }
        else if (_other.CompareTag("Body"))
        {           
            HandleAttack();
        }
    }

    void SetClashPosition(Vector3 _pos)
    {
        Vector3 betweenPlayersPos = Vector3.zero;
        betweenPlayersPos.x = m_player1Controller.transform.position.x + (m_player2Controller.transform.position.x - m_player1Controller.transform.position.x) / 2;
        betweenPlayersPos.y = _pos.y;
        m_clashEffect.transform.position = betweenPlayersPos;
    }

    void HandleCollision()
    {     
         HandlePushBack(m_player1Controller.m_IsAttacking, m_player2Controller.m_IsAttacking);      
    }

    void HandlePushBack(bool _isAttacking1, bool _isAttacking2)
    {
        if (_isAttacking1)
        {
            PushBack(m_player1Controller);
        }
        if (_isAttacking2)
        {
            PushBack(m_player2Controller);
        }
        if (!_isAttacking1 && !_isAttacking2)
        {
            PushBack(m_player1Controller);
            PushBack(m_player2Controller);
        }

        PlayEffect(m_clashEffect);
    }

    void PlayEffect(ParticleSystem _effect)
    {
        AudioManager.Instance.PlayClash("clash");
        _effect.Stop();
        _effect.Play();
    }

    void PushBack(PlayerController _Controller)
    {
        if (_Controller.m_DontMove && !_Controller.m_IsAttacking)
            return;
        _Controller.currentVelocityAmount = _Controller.m_ColliderVelocityAmount;
        _Controller.m_AddExtraVelocity = true;
        _Controller.PlayPushBack();
    }

    bool CheckAttackStates()
    {
        return m_player1Controller.m_IsAttacking == m_player2Controller.m_IsAttacking;
    }

    bool CheckStances()
    {
        return m_player1Controller.m_StanceIndex == m_player2Controller.m_StanceIndex;
    }

    bool CheckAttackTime()
    {
        return (m_player1Controller.m_LastAttackTime <= m_player2Controller.m_LastAttackTime + .1f) && (m_player1Controller.m_LastAttackTime >= m_player2Controller.m_LastAttackTime - .1f);
    }

    void HandleAttack()
    {
        bool sameStance = CheckStances();
        if (sameStance)
        {
            Debug.LogError("attack problem");
        }
        bool sameAttack = CheckAttackStates();
        bool attackInSameTime = CheckAttackTime();

        if (sameAttack)
        {
            if (m_player1Controller.m_IsAttacking)
            {
                if (attackInSameTime)
                {
                    m_player1Controller.Die();
                    m_player2Controller.Die();
                }
                else
                {
                    if (m_player1Controller.m_LastAttackTime < m_player2Controller.m_LastAttackTime)
                    {
                        PushBack(m_player1Controller);
                        m_player1Controller.Win();
                        m_player2Controller.Die();

                    }
                    else
                    {
                        PushBack(m_player2Controller);
                        m_player2Controller.Win();
                        m_player1Controller.Die();

                    }
                }
            }
            else
            {
                m_player1Controller.Die();
                m_player2Controller.Die();
            }
        }
        else
        {
            if (m_player1Controller.m_IsAttacking)
            {
                PushBack(m_player1Controller);
                m_player1Controller.Win();
                m_player2Controller.Die();

            }
            else
            {
                PushBack(m_player2Controller);
                m_player2Controller.Win();
                m_player1Controller.Die();
            }
        }
    }


}
