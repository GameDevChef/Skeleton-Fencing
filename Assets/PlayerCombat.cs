using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public PlayerController MyController;
    public PlayerController EnemyController;


    public bool IsMain;

    public ParticleSystem ClashEffect;
    Vector3 m_clashPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (MyController.IsDead || EnemyController.IsDead)
            return;
       
        if (other.CompareTag("Sword"))
        {
            Debug.Log("sword");
            HandleCollision();
            if (!IsMain)
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

    private void SetClashPosition(Vector3 _pos)
    {
        Vector3 betweenPlayersPos = Vector3.zero;
        betweenPlayersPos.x = MyController.transform.position.x + (EnemyController.transform.position.x - MyController.transform.position.x) / 2;
        betweenPlayersPos.y = _pos.y;
        ClashEffect.transform.position = betweenPlayersPos;
    }

    private void HandleCollision()
    {
       // bool sameStance =  CheckStances();
        //bool sameAttack = CheckAttackStates();
        //if (sameStance)
        {           
             HandlePushBack(MyController.IsAttacking, EnemyController.IsAttacking);                 
        }
    }

    private void HandlePushBack(bool _isAttacking1, bool _isAttacking2)
    {
        
       
        if (_isAttacking1)
        {
            PushBack(MyController);
        }
        if (_isAttacking2)
        {
            PushBack(EnemyController);
        }
        if(!_isAttacking1 && !_isAttacking2)
        {
            PushBack(MyController);
            PushBack(EnemyController);
        }
        if (!IsMain)
            return;

        PlayEffect(ClashEffect);
    }

    private void PlayEffect(ParticleSystem _effect)
    {
        AudioManager.Instance.PlayClash("clash");
        _effect.Stop();
        _effect.Play();
    }

    private void PushBack(PlayerController _Controller)
    {
        if (_Controller.DontMove && !_Controller.IsAttacking)
            return;
        Debug.Log("push");
        _Controller.currentVelocityAmount = _Controller.CollideVelocityAmount;
        _Controller.AddExtraVelocity = true;
        _Controller.m_anim.Play("Push back");
    }

    private bool CheckAttackStates()
    {
        return MyController.IsAttacking == EnemyController.IsAttacking;
    }

    private bool CheckStances()
    {
        return MyController.StanceIndex == EnemyController.StanceIndex;
    }

    private bool CheckAttackTime()
    {
        return (MyController.LastAttackTime <= EnemyController.LastAttackTime + .1f) && (MyController.LastAttackTime >= EnemyController.LastAttackTime - .1f);
    }

    private void HandleAttack()
    {
        if (IsMain)
            return;
        Debug.Log("collision");
        bool sameStance = CheckStances();
        if (sameStance)
        {
            Debug.LogError("attack problem");
        }
        bool sameAttack = CheckAttackStates();
        bool attackInSameTime = CheckAttackTime();

        if (sameAttack)
        {
            if (MyController.IsAttacking)
            {
                if (attackInSameTime)
                {
                   Debug.Log("attack same time");
                    MyController.Die();
                    EnemyController.Die();
                }
                else
                {
                    if (MyController.LastAttackTime < EnemyController.LastAttackTime)
                    {
                       Debug.Log("attack faster");
                        PushBack(MyController);
                        MyController.Win();
                        EnemyController.Die();

                    }
                    else
                    {
                        Debug.Log("slower");
                        PushBack(EnemyController);
                        EnemyController.Win();
                        MyController.Die();

                    }
                }
            }
            else
            {
                Debug.Log("not attacking");
                MyController.Die();
                EnemyController.Die();
            }
        }
        else
        {
            if (MyController.IsAttacking)
            {
               Debug.Log("I attack");
                PushBack(MyController);
                EnemyController.Die();
                MyController.Win();

            }
            else
            {
                Debug.Log("enemy attack");
                PushBack(EnemyController);
                MyController.Die();
                EnemyController.Win();
            }
        }     
    }


}
