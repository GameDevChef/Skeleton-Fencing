using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMover : MonoBehaviour {

    [SerializeField]
    Transform m_target;

    void Update()
    {
        if (m_target == null)
            return;
        transform.position = m_target.position;
    }
}
