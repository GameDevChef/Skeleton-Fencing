using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance;

    public GameObject WinTemplatePrefab;

    public Transform Player1WinGrid;
    public Transform Player2WinGrid;

    public PlayerController Player1;
    public PlayerController Player2;

    int m_player1Score;
    int m_player2Score;

    public float PointsToWin;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(PlayerController _controller)
    {
        if (_controller == Player1)
        {
            m_player1Score++;
            AddTemplate(Player1WinGrid);
        }
          
        else
        {
            m_player2Score++;
            AddTemplate(Player2WinGrid);
        }
           
        if(m_player1Score >= PointsToWin)
        {
            GameManager.Instance.EndGame("Player 1");
            return;
        }

        if (m_player2Score >= PointsToWin)
        {
            GameManager.Instance.EndGame("Player 2");
            return;
        }

        GameManager.Instance.OnEndTurn();
    }

    void AddTemplate(Transform _parent)
    {
        GameObject template = Instantiate(WinTemplatePrefab);
        template.transform.SetParent(_parent, false);
      
    }

}
