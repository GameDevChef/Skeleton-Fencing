using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE
{
    MENU,
    GAMEPLAY
}

public class GameManager : MonoBehaviour {

    public PlayerController Player1;
    public PlayerController Player2;

    public GAME_STATE GameState;

    public static GameManager Instance;

    public GameObject CanvasObj;

    private void Awake()
    {

        Instance = this;
        GameState = GAME_STATE.MENU;
    }

    internal void Play()
    {
        if(Player1.IsReady && Player2.IsReady)
        {
            GameState = GAME_STATE.GAMEPLAY;
            CanvasObj.SetActive(false);
            Player1.InitGame();
            Player2.InitGame();
        }
       
    }

    private void Update()
    {
        if(Player1.transform.position.x < Player2.transform.position.x)
        {
            Player1.isLeft = true;
            Player2.isLeft = false;
        }
        else
        {
            Player1.isLeft = false;
            Player2.isLeft = true;
        }
            
    }
}
