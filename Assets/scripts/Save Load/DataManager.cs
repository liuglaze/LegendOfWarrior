using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;
using System.Net.WebSockets;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("事件监听")]
    public VoidEventSO saveDataEventSO;
    public VoidEventSO loadDataEventSO;
    public List<ISaveable> saveableDataList = new List<ISaveable>();
    public List<ISaveable> saveableSceneList = new List<ISaveable>();
    public Data saveData;
    private string jsonFolder;
    private void Awake()
    {
        
       if(instance == null)
            instance = this;
       else Destroy(this.gameObject);
        saveData = new Data();
        //这是固定路径，注意这只是名字，还未在硬盘中创建对应的文件夹
        jsonFolder = Application.persistentDataPath + "/SAVE DATA";
        //开始就读取文件中的data
        ReadSavedData();
    }
    private void OnEnable()
    {
        saveDataEventSO.OnEventRaised += Save;
        loadDataEventSO.OnEventRaised += Load;
    }
    private void OnDisable()
    {
        saveDataEventSO.OnEventRaised -= Save;
        loadDataEventSO.OnEventRaised -= Load;
    }
    private void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableDataList.Contains(saveable))
        {
            saveableDataList.Add(saveable);
        }
    }

    public void RegisterSceneSaveData(ISaveable saveable)
    {
        if (!saveableSceneList.Contains(saveable))
        {
            saveableSceneList.Add(saveable);
        }
    }




    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableDataList.Remove(saveable);
    }

    public void UnRegisterSceneSaveData(ISaveable saveable)
    {
        saveableSceneList.Remove(saveable);
    }
    public void Save()
    {
        foreach(var saveable in saveableDataList)
        {
            saveable.GetSaveData(saveData);
        }
        
        foreach (var saveable in saveableSceneList)
        {
            saveable.GetSaveData(saveData);
            
        }
        //要写的文件的位置+名字
        var resulutPath = jsonFolder + "data.sav";
        var jsonData=JsonConvert.SerializeObject(saveData);
        //如果不存在这个文件夹就创建一个
        if(!File.Exists(resulutPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        //将序列化的data写入data.sav
        File.WriteAllText(resulutPath, jsonData);
    }
    public void Load()
    {

        StartCoroutine(Load1());

    }
    private IEnumerator Load1()
    {
        //先加载场景再读取数据，防止读取的数据又被加载场景覆盖掉
        foreach (var saveble in saveableSceneList)
        {
            saveble.LoadData(saveData);
        }
        yield return new WaitForSeconds(0.35f);
        foreach (var saveble in saveableDataList)
        {
            saveble.LoadData(saveData);
        }
    }
    private void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";
        if(File.Exists(resultPath))
        {
           var stringData=File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            saveData=jsonData;
        }
    }
}
