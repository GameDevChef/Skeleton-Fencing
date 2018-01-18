using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    [SerializeField]
    CombatManager m_combatManager;

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.transform.root == transform.root)
            return;

        m_combatManager.HandleCombat(_other);
    }
}
