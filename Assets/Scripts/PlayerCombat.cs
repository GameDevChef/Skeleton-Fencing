using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    PlayerController m_myController;

    [SerializeField]
    PlayerController m_enemyController;

    [SerializeField]
    ParticleSystem m_clashEffect;

    [Header("Variables")]

    [SerializeField]
    bool m_isMain;

    Vector3 m_clashPosition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_myController.m_IsDead || m_enemyController.m_IsDead)
            return;
       
        if (other.CompareTag("Sword"))
        {
            HandleCollision();
            if (!m_isMain)
                return;
            SetClashPosition(other.transform.position);
           
        }
        else if (other.CompareTag("Body"))
        {
            if (other.transform.root == transform.root)
                return;
            HandleAttack();
        }
    }

    void SetClashPosition(Vector3 _pos)
    {
        Vector3 betweenPlayersPos = Vector3.zero;
        betweenPlayersPos.x = m_myController.transform.position.x + (m_enemyController.transform.position.x - m_myController.transform.position.x) / 2;
        betweenPlayersPos.y = _pos.y;
        m_clashEffect.transform.position = betweenPlayersPos;
    }

    void HandleCollision()
    {
        {           
             HandlePushBack(m_myController.m_IsAttacking, m_enemyController.m_IsAttacking);                 
        }
    }

    void HandlePushBack(bool _isAttacking1, bool _isAttacking2)
    {              
        if (_isAttacking1)
        {
            PushBack(m_myController);
        }
        if (_isAttacking2)
        {
            PushBack(m_enemyController);
        }
        if(!_isAttacking1 && !_isAttacking2)
        {
            PushBack(m_myController);
            PushBack(m_enemyController);
        }
        if (!m_isMain)
            return;

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
        return m_myController.m_IsAttacking == m_enemyController.m_IsAttacking;
    }

    bool CheckStances()
    {
        return m_myController.m_StanceIndex == m_enemyController.m_StanceIndex;
    }

    bool CheckAttackTime()
    {
        return (m_myController.m_LastAttackTime <= m_enemyController.m_LastAttackTime + .1f) && (m_myController.m_LastAttackTime >= m_enemyController.m_LastAttackTime - .1f);
    }

    void HandleAttack()
    {
        if (!m_isMain)
            return;
        bool sameStance = CheckStances();
        if (sameStance)
        {
            Debug.LogError("attack problem");
        }
        bool sameAttack = CheckAttackStates();
        bool attackInSameTime = CheckAttackTime();

        if (sameAttack)
        {
            if (m_myController.m_IsAttacking)
            {
                if (attackInSameTime)
                {
                    m_myController.Die();
                    m_enemyController.Die();
                }
                else
                {
                    if (m_myController.m_LastAttackTime < m_enemyController.m_LastAttackTime)
                    {
                        PushBack(m_myController);
                        m_myController.Win();
                        m_enemyController.Die();

                    }
                    else
                    {
                        PushBack(m_enemyController);
                        m_enemyController.Win();
                        m_myController.Die();

                    }
                }
            }
            else
            {
                m_myController.Die();
                m_enemyController.Die();
            }
        }
        else
        {
            if (m_myController.m_IsAttacking)
            {
                PushBack(m_myController);
                m_enemyController.Die();
                m_myController.Win();

            }
            else
            {
                PushBack(m_enemyController);
                m_myController.Die();
                m_enemyController.Win();
            }
        }     
    }


}
