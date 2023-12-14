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
    [Header("事件监听")]
    public SceneLoadEventSO loadSceneEventSO;
    public VoidEventSO newGameEventSO;
    public VoidEventSO backToMenuEventSO;
    [Header("场景")]
    private GameSceneSO sceneToLoad;
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;
    [SerializeField]public GameSceneSO currentLoadedScene;
    private Vector3 postionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    [Header("广播")]
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
        
        //开始时直接启动menu
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
    //开始游戏
    public void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //通知了playercontroller没死
        loadSceneEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true); 
    }
    //死了后返回菜单
    private void OnBackToMenu()
    {
        Time.timeScale = 1;
        sceneToLoad = menuScene;
        loadSceneEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }
    //loadSceneEvent的实现函数,实现函数还有playercontroller里的，让人物无法跳跃和移动
    //核心函数loadNewScene
    //若当前有场景，卸载场景后(这里会完成渐入渐出)再loadnew
    //不然直接load(应用场景为menu->在start里)
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
            //TODO:渐入渐出
            fadeEventSO.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
        //虽然是卸载场景，但参数写的却是要去的场景。
        //原因是卸载本身并不重要，重要的是根据卸载后的场景是什么来决定要干什么，比如卸载后去菜单就不显示ui
        //通知了ui
        //感觉有点冗余，在loadscene那里写也完全可以
        unloadedSceneEventSO.RaiseLoadRequestEvent(sceneToLoad,postionToGo, true);
        //卸载场景
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        //不显示人物
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        Time.timeScale = 1;
        //加载场景
        var loadingOpration =sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        //加载场景完毕后
        loadingOpration.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        Time.timeScale = 1;
        currentLoadedScene = sceneToLoad;
        //转移人物坐标
        playerTrans.position = postionToGo;
        
        if(fadeScreen)
        {
            //渐出
            fadeEventSO.FadeOut(fadeDuration);
        }
        //显示人物
        playerTrans.gameObject.SetActive(true);
        isLoading = false;
        if(currentLoadedScene.sceneType!=SceneType.Menu)
            //加载完成后要做的事，比如playercontroller可以移动了
        afterSceneLoadedEvent.RaiseEvent();
    }

    

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    //调用data manager的注册函数，将自己注册进saveable的list注册表里
    public void RegisterSaveData()
    {
        DataManager.instance.RegisterSceneSaveData(this);
    }
    public void UnRegisterSaveData()
    {
        DataManager.instance.UnRegisterSceneSaveData(this);
    }
    //data manager会调用getsave函数->调用list里的每个savable的getsavedata函数
    //datamanager将自己的data传给这些函数，savable自己将数据传进data里
    //而这里在data里内置了函数，方便我们将场景传进data里
    public void GetSaveData(Data data)
    {
        data.SaveGamescene(currentLoadedScene);
    }
    //同上，datamanger调用每一个loaddata函数，将data传给load，savable自己使用data里存好的数据
    //死亡时继续游戏和菜单里继续游戏
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
