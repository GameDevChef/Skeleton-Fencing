using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE
{
    MENU,
    GAMEPLAY,
    RESET
}

public class GameManager : MonoBehaviour {

    public PlayerController Player1;
    public PlayerController Player2;

    public GameObject Player1Prefab;
    public GameObject Player2Prefab;

    public GAME_STATE GameState;

    public static GameManager Instance;

    public GameObject PlayersCanvas;


    private void Awake()
    {

        Instance = this;
        GameState = GAME_STATE.MENU;
    }

    internal void Play()
    {
        if(Player1.IsReady && Player2.IsReady)
        {
            Player1Prefab = Player1.gameObject;
            Player2Prefab = Player2.gameObject;
            GameState = GAME_STATE.GAMEPLAY;
            PlayersCanvas.SetActive(false);
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


    void ResetPlayers()
    {
        Player1.Reset();
        Player2.Reset();
        GameState = GAME_STATE.GAMEPLAY;
    }

    internal void OnEndTurn()
    {
        
        //Invoke("ChangeToReset", .1f);
        Invoke("ResetPlayers", 2f);
       //ChangeToReset();
    }


    void ChangeToReset()
    {
        GameState = GAME_STATE.RESET;
    }




}


