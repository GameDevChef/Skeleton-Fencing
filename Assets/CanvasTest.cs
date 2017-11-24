using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTest : MonoBehaviour {

	public GameObject Character;


    private void Start()
    {
        Character.layer = 8;
    }

}
