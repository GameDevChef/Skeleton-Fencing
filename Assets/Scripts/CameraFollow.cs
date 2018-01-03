using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Transform m_player1Transform;

    [SerializeField]
    Transform m_player2Transform;

    Transform m_transform;

    Camera mainCamera;

    [Header("Variables")]

    [SerializeField]
    float m_minCameraSize;

    [SerializeField]
    float m_cameraSizeOffset;

    bool m_follow;

    float m_cameraSize;

    float m_distance; 

    Vector3 m_middlePos;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_transform = GetComponent<Transform>();
        m_middlePos = m_transform.position;
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.m_GameState != GAME_STATE.GAMEPLAY)
            return;
        SetPosition();
    }

    void SetPosition()
    {
        m_distance = m_player2Transform.transform.position.x - m_player1Transform.transform.position.x;
        m_middlePos.x = m_player1Transform.transform.position.x + (m_distance) / 2;
        m_cameraSize = m_distance * Screen.height / Screen.width + m_cameraSizeOffset;
        if(m_cameraSize < m_minCameraSize)
        {
            m_cameraSize = m_minCameraSize;
        }
        m_middlePos.y = -2.5f + mainCamera.orthographicSize;
        m_transform.position = Vector3.Lerp(m_transform.position, m_middlePos, Time.deltaTime * 10f);

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, m_cameraSize, Time.deltaTime * 10f);    
    }
}
