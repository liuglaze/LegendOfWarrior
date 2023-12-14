using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public DataManager dataManager;
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    public GameObject player;
    [Header("�¼�����")]
    public SceneLoadEventSO loadSceneEventSO;
    public VoidEventSO newGameEventSO;
    public VoidEventSO backToMenuEventSO;
    [Header("����")]
    private GameSceneSO sceneToLoad;
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;
    [SerializeField]public GameSceneSO currentLoadedScene;
    private Vector3 postionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    [Header("�㲥")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEventSO;
    public SceneLoadEventSO unloadedSceneEventSO;
    private void Awake()
    {
        
    }
   
    private void OnEnable()
    {
        RegisterSaveData();
        loadSceneEventSO.LoadRequestEvent += OnLoadREquestEvent;
        newGameEventSO.OnEventRaised += NewGame;
        backToMenuEventSO.OnEventRaised += OnBackToMenu;
        
    }
    private void Start()
    {
        
        //��ʼʱֱ������menu
        loadSceneEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        
    }
    private void OnDisable()
    {
        loadSceneEventSO.LoadRequestEvent -= OnLoadREquestEvent;
        newGameEventSO.OnEventRaised -= NewGame;
        UnRegisterSaveData();
        backToMenuEventSO.OnEventRaised -= OnBackToMenu;
        //ISaveable saveable = this;
        //saveable.UnRegisterSaveData();
    }
    //��ʼ��Ϸ
    public void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //֪ͨ��playercontrollerû��
        loadSceneEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true); 
    }
    //���˺󷵻ز˵�
    private void OnBackToMenu()
    {
        Time.timeScale = 1;
        sceneToLoad = menuScene;
        loadSceneEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }
    //loadSceneEvent��ʵ�ֺ���,ʵ�ֺ�������playercontroller��ģ��������޷���Ծ���ƶ�
    //���ĺ���loadNewScene
    //����ǰ�г�����ж�س�����(�������ɽ��뽥��)��loadnew
    //��Ȼֱ��load(Ӧ�ó���Ϊmenu->��start��)
    private void OnLoadREquestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        Time.timeScale = 1;
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        sceneToLoad = locationToLoad;
        postionToGo = posToGo;
        this.fadeScreen=fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            Time.timeScale = 1;
            LoadNewScene();
        }
        
        Debug.Log(sceneToLoad.sceneReference.SubObjectName);
    }
    private IEnumerator UnLoadPreviousScene()
    {
        Time.timeScale = 1;
        if (fadeScreen)
        {
            //TODO:���뽥��
            fadeEventSO.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
        //��Ȼ��ж�س�����������д��ȴ��Ҫȥ�ĳ�����
        //ԭ����ж�ر�������Ҫ����Ҫ���Ǹ���ж�غ�ĳ�����ʲô������Ҫ��ʲô������ж�غ�ȥ�˵��Ͳ���ʾui
        //֪ͨ��ui
        //�о��е����࣬��loadscene����дҲ��ȫ����
        unloadedSceneEventSO.RaiseLoadRequestEvent(sceneToLoad,postionToGo, true);
        //ж�س���
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        //����ʾ����
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        Time.timeScale = 1;
        //���س���
        var loadingOpration =sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        //���س�����Ϻ�
        loadingOpration.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        Time.timeScale = 1;
        currentLoadedScene = sceneToLoad;
        //ת����������
        playerTrans.position = postionToGo;
        
        if(fadeScreen)
        {
            //����
            fadeEventSO.FadeOut(fadeDuration);
        }
        //��ʾ����
        playerTrans.gameObject.SetActive(true);
        isLoading = false;
        if(currentLoadedScene.sceneType!=SceneType.Menu)
            //������ɺ�Ҫ�����£�����playercontroller�����ƶ���
        afterSceneLoadedEvent.RaiseEvent();
    }

    

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    //����data manager��ע�ắ�������Լ�ע���saveable��listע�����
    public void RegisterSaveData()
    {
        DataManager.instance.RegisterSceneSaveData(this);
    }
    public void UnRegisterSaveData()
    {
        DataManager.instance.UnRegisterSceneSaveData(this);
    }
    //data manager�����getsave����->����list���ÿ��savable��getsavedata����
    //datamanager���Լ���data������Щ������savable�Լ������ݴ���data��
    //��������data�������˺������������ǽ���������data��
    public void GetSaveData(Data data)
    {
        data.SaveGamescene(currentLoadedScene);
    }
    //ͬ�ϣ�datamanger����ÿһ��loaddata��������data����load��savable�Լ�ʹ��data���õ�����
    //����ʱ������Ϸ�Ͳ˵��������Ϸ
    public void LoadData(Data data)
    {
        Time.timeScale = 1;
        Time.timeScale = 1;
        Time.timeScale = 1;
        Time.timeScale = 1;
        player.GetComponent<Character>().over = false;
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if(data.characterPosDict.ContainsKey(playerID))
        {
            postionToGo = data.characterPosDict[playerID].ToVector3();
            sceneToLoad = data.GetSavedScene();
            OnLoadREquestEvent(sceneToLoad, postionToGo, true);
        }
    }


}
