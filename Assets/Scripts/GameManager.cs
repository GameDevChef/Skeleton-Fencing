using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GAME_STATE
{
    MENU,
    GAMEPLAY,
    RESET
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Header("References")]

    [SerializeField]
    PlayerController m_player1Controller;

    [SerializeField]
    PlayerController m_player2Controller;

    [SerializeField]
    InputHandler m_player1InputHandler;

    [SerializeField]
    InputHandler m_player2InputHandler;

    [SerializeField]
    GameObject m_winScreen;

    [SerializeField]
    Text m_winText;

    [HideInInspector]
    public GameObject m_Player1Prefab;

    [HideInInspector]
    public GameObject m_Player2Prefab;

    //[HideInInspector]
    public GAME_STATE m_GameState;

    [SerializeField]
    GameObject m_playersCanvases;

    void Awake()
    {
        Instance = this;
        m_GameState = GAME_STATE.MENU;
    }

    public void Play()
    {
        if(m_player1Controller.m_IsReady && m_player2Controller.m_IsReady)
        {
            m_Player1Prefab = m_player1Controller.gameObject;
            m_Player2Prefab = m_player2Controller.gameObject;
            m_player1Controller.OnStart();
            m_player2Controller.OnStart();
            m_GameState = GAME_STATE.GAMEPLAY;
            m_playersCanvases.SetActive(false);
            m_player1Controller.InitGame();
            m_player2Controller.InitGame();
        }
       
    }

    public void EndGame(string _playerName)
    {
        m_GameState = GAME_STATE.RESET;
        m_winScreen.SetActive(true);
        m_winText.text = _playerName + " wins.";
    }

    void Update()
    {
        if(m_player1Controller.transform.position.x < m_player2Controller.transform.position.x)
        {
            m_player1Controller.m_IsLeft = true;
            m_player2Controller.m_IsLeft = false;
        }
        else
        {
            m_player1Controller.m_IsLeft = false;
            m_player2Controller.m_IsLeft = true;
        }           
    }

    void ResetPlayers()
    {
        m_player1Controller.Reset();
        m_player2Controller.Reset();
        m_GameState = GAME_STATE.GAMEPLAY;
    }

    public void OnEndTurn()
    {
        if (m_GameState == GAME_STATE.RESET)
            return;
        Invoke("ResetPlayers", 2f);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    } 
}


