using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("广播")]
    public VoidEventSO pauseEventSO;
    [Header("事件监听")]
    public CoinsUI coinsUI;
    public CharacterEventSO healthEvent;
    public PlayerStateBar playerStateBar;
    public GameObject armourUI;
    public SceneLoadEventSO unLoadEventSO;
    public VoidEventSO loadDataEventSO;
    public VoidEventSO gameOverEventSO;
    public VoidEventSO backToMenuEventSO;
    public FloatEventSO syncVolumeEventSO;

    [Header("组件")]
    public PlayerController playerController;
    public Character character;
    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public GameObject pausePanel;
    public GameObject sword;
    public GameObject boot;
    public Button settingButton;
    public Slider volumeSlider;
    public Button swordButton;
    public Button bootButton;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        settingButton.onClick.AddListener(TogglePausePanel);
        swordButton.onClick.AddListener(RemoveSword);
        bootButton.onClick.AddListener(RemoveBoot);
    }

    private void RemoveBoot()
    {
       if(boot.activeInHierarchy)
        {
            boot.SetActive(false);
        
        }
    }

    private void RemoveSword()
    {
        if (sword.activeInHierarchy)
        {
            sword.SetActive(false);
        }
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unLoadEventSO.LoadRequestEvent += OnUnLoadEvent;
        loadDataEventSO.OnEventRaised += OnLoadDataEvent;
        gameOverEventSO.OnEventRaised += OnGameOverEvent;
        backToMenuEventSO.OnEventRaised += OnLoadDataEvent;
        syncVolumeEventSO.OnEventRaised += OnSyncVolumeEvent;
    }
    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pauseEventSO.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        character.invulnerable = true;
        character.over=true;
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
        StartCoroutine(OnLoadData());
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxhealth;
        playerStateBar.OnHealthCchange(percentage);
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unLoadEventSO.LoadRequestEvent -= OnUnLoadEvent;
        loadDataEventSO.OnEventRaised -= OnLoadDataEvent;
        gameOverEventSO.OnEventRaised -= OnGameOverEvent;
        backToMenuEventSO.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEventSO.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void OnUnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        if(sceneToLoad.sceneType==SceneType.Menu)
        {
            playerStateBar.gameObject.SetActive(false);
            coinsUI.gameObject.SetActive(false);
            armourUI.SetActive(false);
        }
        else
        {
            playerStateBar.gameObject.SetActive(true);
            coinsUI.gameObject.SetActive(true);
            armourUI.SetActive(true);
        }
    }
    private IEnumerator OnLoadData()
    {
        yield return new WaitForSeconds(0.35f);
        gameOverPanel.SetActive(false);
    }
}
