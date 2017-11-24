using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public PlayerController EnemyController;
    PlayerController m_myController;

    public float collideRange;

    public float attackRange;

    public float swordRange;

    public float currentRange;

    public float Distance;

    bool m_isInCollisionRange;
    bool m_isInAttackRange;
    bool m_isInSwordRange;

    public float ColldieOffset;
    float currentCollideOffset;
   

    private void Awake()
    {
        m_myController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (m_myController.IsDead || EnemyController.IsDead)
            return;
        Distance = Vector3.Distance(transform.position, EnemyController.transform.position);
        currentRange = (m_myController.IsAttacking) ? attackRange : swordRange;

        m_isInCollisionRange = (Distance <= collideRange) ? true : false;
        m_isInAttackRange = (Distance <= currentRange) ? true : false;



        HandleCollide();
    }

    private void HandleCollide()
    {
        currentCollideOffset += Time.deltaTime;
        if (!m_isInCollisionRange)
            return;
        if(m_myController.StanceIndex == EnemyController.StanceIndex)
        {
            Collide();
        }
        else
        {           
            Attack();
        }
    }

    private void Attack()
    {
        if (!m_isInAttackRange)
            return;
        
        if(m_myController.IsAttacking == EnemyController.IsAttacking)
        {
            if(m_myController.IsAttacking == true)
            {
                Debug.Log("both attacking");
                if (m_myController.LastAttackTime < EnemyController.LastAttackTime - .1f)
                {
                    Debug.Log(m_myController.isLeft + " was faster");
                    EnemyController.gameObject.SetActive(false);
                    EnemyController.IsDead = true;
                }
                else if (m_myController.LastAttackTime > EnemyController.LastAttackTime + .1f)
                {
                    Debug.Log(m_myController.isLeft + " was slower");
                    m_myController.gameObject.SetActive(false);
                    m_myController.IsDead = true;
                }
                else
                {
                    Debug.Log(m_myController.isLeft + " same time");
                    EnemyController.gameObject.SetActive(false);
                    m_myController.gameObject.SetActive(false);
                    EnemyController.IsDead = true;
                    m_myController.IsDead = true;
                }
            }
            else
            {
                Debug.Log(m_myController.isLeft + " run int each other");
                EnemyController.gameObject.SetActive(false);
                m_myController.gameObject.SetActive(false);
                EnemyController.IsDead = true;
                m_myController.IsDead = true;
            }                       
        }
        else
        {
            if (m_myController.IsAttacking)
            {
                Debug.Log(m_myController.isLeft + " was attacking");
                EnemyController.gameObject.SetActive(false);
                EnemyController.IsDead = true;
            }
            else
            {
                Debug.Log(m_myController.isLeft + " wasnt attacking");
                m_myController.gameObject.SetActive(false);
                m_myController.IsDead = true;
            }
        }
    }

    private void Collide()
    {
        if (currentCollideOffset < ColldieOffset)
            return;
        currentCollideOffset = 0f;
        if(m_myController.IsAttacking == EnemyController.IsAttacking)
        {
            Debug.Log("collide same");
            m_myController.currentVelocityAmount = m_myController.CollideVelocityAmount;
            m_myController.AddExtraVelocity = true;
            m_myController.m_anim.Play("Push back");
            EnemyController.currentVelocityAmount = m_myController.CollideVelocityAmount;
            EnemyController.AddExtraVelocity = true;
            EnemyController.m_anim.Play("Push back");
        }
        else
        {
            if (m_myController.IsAttacking)
            {
                Debug.Log("collide attack");
                m_myController.currentVelocityAmount = m_myController.CollideVelocityAmount;
                m_myController.AddExtraVelocity = true;
                m_myController.m_anim.Play("Push back");
            }
            else
            {
                Debug.Log("collide defend");
                EnemyController.currentVelocityAmount = m_myController.CollideVelocityAmount;
                EnemyController.AddExtraVelocity = true;
                EnemyController.m_anim.Play("Push back");
            }
        }
       
    }

    void Die()
    {
        
        EnemyController.gameObject.SetActive(false);
    }
}
