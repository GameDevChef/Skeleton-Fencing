using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMover : MonoBehaviour {

    public Transform Target;

    private void Update()
    {
        if (Target == null)
            return;
        transform.position = Target.position;
    }
}
