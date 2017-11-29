using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Transform m_transform;
    Camera mainCamera;

    public Transform Player1;
    public Transform Player2;

    public float MinCameraSize;
    public float CamaraSizeOffset;

    bool m_follow;

    float cameraSize;
    float m_distance;

    

    Vector3 m_middlePos;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        m_middlePos = m_transform.position;
        mainCamera = Camera.main;
    }    

    private void LateUpdate()
    {
        if (GameManager.Instance.GameState != GAME_STATE.GAMEPLAY)
            return;
        SetPosition();
    }

    private void SetPosition()
    {
        m_distance = Player2.transform.position.x - Player1.transform.position.x;
        m_middlePos.x = Player1.transform.position.x + (m_distance) / 2;
        cameraSize = m_distance * Screen.height / Screen.width + CamaraSizeOffset;
        if(cameraSize < MinCameraSize)
        {
            cameraSize = MinCameraSize;
        }
        m_middlePos.y = -2.5f + mainCamera.orthographicSize;
        m_transform.position = Vector3.Lerp(m_transform.position, m_middlePos, Time.deltaTime * 10f);

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSize, Time.deltaTime * 10f);
       
    }
}
