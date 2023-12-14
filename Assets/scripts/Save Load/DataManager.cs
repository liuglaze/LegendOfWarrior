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
    [Header("�¼�����")]
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
        //���ǹ̶�·����ע����ֻ�����֣���δ��Ӳ���д�����Ӧ���ļ���
        jsonFolder = Application.persistentDataPath + "/SAVE DATA";
        //��ʼ�Ͷ�ȡ�ļ��е�data
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
        //Ҫд���ļ���λ��+����
        var resulutPath = jsonFolder + "data.sav";
        var jsonData=JsonConvert.SerializeObject(saveData);
        //�������������ļ��оʹ���һ��
        if(!File.Exists(resulutPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        //�����л���dataд��data.sav
        File.WriteAllText(resulutPath, jsonData);
    }
    public void Load()
    {

        StartCoroutine(Load1());

    }
    private IEnumerator Load1()
    {
        //�ȼ��س����ٶ�ȡ���ݣ���ֹ��ȡ�������ֱ����س������ǵ�
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
