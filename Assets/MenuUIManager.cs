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

    public MENU_STATE MenuState;

    AudioManager m_audioManager;

    public GameObject IntroGO;
    public GameObject MenuGO;
    public GameObject CreditsGO;
    public GameObject InsultGO;

    public List<RectTransform> MenuItemsList;
    public RectTransform Selector;
    int m_selectorIndex;

    public string MainSceneName = "Main";

    private bool m_downMenu;
    private bool m_upMenu;
    private bool m_apply;

    public string ApplyButtonName;
    public string UpButtonName;
    public string DownButtonName;

    private void Start()
    {
        m_audioManager = AudioManager.Instance;
        MenuState = MENU_STATE.INTRO;
        m_selectorIndex = 0;
        SetSelectorPosition();
    }

    private void Update()
    {
        GetMenuInput();
        PlayAudio();
        switch (MenuState)
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

    private void PlayAudio()
    {
        if (m_upMenu)
            m_audioManager.PlayMenu("Up Down");

        if (m_downMenu)
            m_audioManager.PlayMenu("Up Down");

        if (m_apply)
            m_audioManager.PlayMenu("Apply");
    }

    private void SetSelectorPosition()
    {     
        Selector.anchoredPosition = MenuItemsList[m_selectorIndex].anchoredPosition;
    }

    private void MenuTick()
    {
        if (m_upMenu)
        {
            m_selectorIndex--;
            if (m_selectorIndex < 0)
                m_selectorIndex = MenuItemsList.Count - 1;

            SetSelectorPosition();
        }


        if (m_downMenu)
        {
            m_selectorIndex++;
            if (m_selectorIndex > MenuItemsList.Count - 1)
                m_selectorIndex = 0;

            SetSelectorPosition();
        }

        if (m_apply)
            ApplyMenu();

       

    }

    private void ApplyMenu()
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

    private void ExitGame()
    {
       
    }

    private void PlayGame()
    {
        SetGOsStates(false, false, false, true);
        StartCoroutine(LoadLevelAsyncCO(MainSceneName, 2f));
    }

    private void ChangeState(MENU_STATE _targetState)
    {
        MenuState = _targetState;
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
        m_downMenu = Input.GetButtonDown((DownButtonName));
        m_upMenu = Input.GetButtonDown((UpButtonName));
        m_apply = Input.GetButtonDown((ApplyButtonName));
    }

    void SetGOsStates(bool _intro, bool _menu, bool _credits, bool _insult)
    {
        IntroGO.SetActive(_intro);
        MenuGO.SetActive(_menu);
        CreditsGO.SetActive(_credits);
        InsultGO.SetActive(_insult);
    }

    private IEnumerator LoadLevelAsyncCO(string _sceneName, float _deley)
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
