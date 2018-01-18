using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance;

    [Header("References")]

    [SerializeField]
    GameObject m_winTemplatePrefab;

    [SerializeField]
    Transform m_player1WinGrid;

    [SerializeField]
    Transform m_player2WinGrid;

    [SerializeField]
    PlayerController m_playerController1;

    [SerializeField]
    PlayerController m_playerController2;

    [Header("Variables")]

    [SerializeField]
    float m_pointsToWin;

    int m_player1Score;

    int m_player2Score;


    void Awake()
    {
        Instance = this;
    }

    public void AddScore(PlayerController _controller)
    {
        if (_controller == m_playerController1)
        {
            m_player1Score++;
            AddTemplate(m_player1WinGrid);
        }
          
        else
        {
            m_player2Score++;
            AddTemplate(m_player2WinGrid);
        }
           
        if(m_player1Score >= m_pointsToWin)
        {
            GameManager.Instance.EndGame("Player 1");
            return;
        }

        if (m_player2Score >= m_pointsToWin)
        {
            GameManager.Instance.EndGame("Player 2");
            return;
        }

        
    }

    void AddTemplate(Transform _parent)
    {
        GameObject template = Instantiate(m_winTemplatePrefab);
        template.transform.SetParent(_parent, false);
      
    }

}
