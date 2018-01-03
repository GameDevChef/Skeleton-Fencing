using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MENU_STATE
{
    INTRO,
    MENU,
    CREDITS
}

public class MenuUIManager : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GameObject m_introGO;

    [SerializeField]
    GameObject m_menuGO;

    [SerializeField]
    GameObject m_creditsGO;

    [SerializeField]
    GameObject m_insultGO;

    [SerializeField]
    List<RectTransform> m_menuListItems;

    [SerializeField]
    RectTransform m_selector;

    MENU_STATE m_menuState;

    AudioManager m_audioManager;

    [Header("Variables")]

    [SerializeField]
    public string m_applyButtonName;

    [SerializeField]
    public string m_upButtonName;

    [SerializeField]
    public string m_downButtonName;

    [SerializeField]
    public string m_yAxisName;

    int m_selectorIndex;

    string m_mainSceneName = "Main";

    float vertical;

    bool m_downMenu;

    bool m_upMenu;

    bool m_apply;

    bool m_canMoveVertical;

    bool m_isLoadingGame;



    void Start()
    {
        m_audioManager = AudioManager.Instance;
        m_menuState = MENU_STATE.INTRO;
        m_selectorIndex = 0;
        SetSelectorPosition();
        m_canMoveVertical = true;
    }

    void Update()
    {
        GetMenuInput();
        PlayAudio();
        switch (m_menuState)
        {
            case MENU_STATE.INTRO:
                if (m_apply)
                {
                    ChangeState(MENU_STATE.MENU);
                }
                break;
            case MENU_STATE.MENU:
                MenuTick();
                break;
            case MENU_STATE.CREDITS:
                if (m_apply)
                {
                    ChangeState(MENU_STATE.MENU);
                }
                break;
            default:
                break;
        }
    }

    void PlayAudio()
    {
        if (m_isLoadingGame)
            return;
        if (m_upMenu)
            m_audioManager.PlayMenu("Up Down");

        if (m_downMenu)
            m_audioManager.PlayMenu("Up Down");

        if (m_apply)
            m_audioManager.PlayMenu("Apply");
    }

    void SetSelectorPosition()
    {     
        m_selector.anchoredPosition = m_menuListItems[m_selectorIndex].anchoredPosition;
    }

    void MenuTick()
    {      
        if (m_upMenu)
        {
            if (!m_canMoveVertical)
                return;
            m_selectorIndex--;
            if (m_selectorIndex < 0)
                m_selectorIndex = m_menuListItems.Count - 1;
            m_canMoveVertical = false;
            SetSelectorPosition();
        }

        if (m_downMenu)
        {
            if (!m_canMoveVertical)
                return;
            m_selectorIndex++;
            if (m_selectorIndex > m_menuListItems.Count - 1)
                m_selectorIndex = 0;
            m_canMoveVertical = false;
            SetSelectorPosition();
        }

        if (m_apply)
            ApplyMenu();     

    }

    void ApplyMenu()
    {
        switch (m_selectorIndex)
        {
            case 0:
                PlayGame();
                break;
            case 1:
                ChangeState(MENU_STATE.CREDITS);
                break;
            case 2:
                ExitGame();
                break;
            default:
                break;
        }
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void PlayGame()
    {
        if (m_isLoadingGame)
            return;
        m_isLoadingGame = true;
        SetGOsStates(false, false, false, true);
        StartCoroutine(LoadLevelAsyncCO(m_mainSceneName, 2f));
    }

    void ChangeState(MENU_STATE _targetState)
    {
        m_menuState = _targetState;
        switch (_targetState)
        {
            case MENU_STATE.INTRO:
                SetGOsStates(true, false, false, false);              
                break;
            case MENU_STATE.MENU:
                SetGOsStates(false, true, false, false);
                break;
            case MENU_STATE.CREDITS:
                SetGOsStates(false, false, true, false);
                break;
            default:
                break;
        }
    }

    void GetMenuInput()
    {           
        vertical = Input.GetAxis(m_yAxisName);
        if(!m_canMoveVertical)
        {
            if (vertical == 0)
            {
                m_canMoveVertical = true;
            }                
        }      
        m_downMenu = vertical > 0;
        m_upMenu = vertical < 0;
        m_apply = Input.GetButtonDown((m_applyButtonName));
    }

    void SetGOsStates(bool _intro, bool _menu, bool _credits, bool _insult)
    {
        m_introGO.SetActive(_intro);
        m_menuGO.SetActive(_menu);
        m_creditsGO.SetActive(_credits);
        m_insultGO.SetActive(_insult);
    }

    IEnumerator LoadLevelAsyncCO(string _sceneName, float _deley)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName);
  
        operation.allowSceneActivation = false;

        while (operation.progress < .9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_deley);
        operation.allowSceneActivation = true;
    }
}
