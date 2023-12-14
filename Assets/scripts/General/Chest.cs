using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable,ISaveable
{
    public SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    public GameObject owner;
    public GameObject coin;
    public GameObject Armour;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }

    private void OnEnable()
    {
        //spriteRenderer.sprite=isDone?openSprite : closeSprite;
        RegisterSaveData();
    }
    private void OnDisable()
    {
        ISaveable saveable = this;
        UnRegisterSaveData();
    }
    public void TriggerAction(GameObject owner)
    {
        this.owner = owner;
        Debug.Log("Open Chest!");
        if(!isDone)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        coin.SetActive(true);
        Armour.SetActive(true);
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
        GetComponent<AudioDefination>()?.PlayAudioClip();
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if (data.boolSaveData.ContainsKey(GetDataID().ID))
        {
            data.boolSaveData[GetDataID().ID] = isDone;
            
        }
        else
        {

            data.boolSaveData.Add(GetDataID().ID, isDone);
        }
    }

    public void LoadData(Data data)
    {
        if (data.boolSaveData.ContainsKey(GetDataID().ID))
        {
            isDone = data.boolSaveData[GetDataID().ID];
            spriteRenderer.sprite = isDone ? openSprite : closeSprite;
            this.gameObject.tag = isDone ? "Untagged" : "interactable";
        }
    }

    public void RegisterSaveData()
    {
        DataManager.instance.RegisterSaveData(this);
    }

    public void UnRegisterSaveData()
    {
        DataManager.instance.UnRegisterSaveData(this);
    }
}
