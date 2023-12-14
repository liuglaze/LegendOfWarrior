using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour,ISaveable
{
    [Header("监听事件")]
    public VoidEventSO newGameEventSO;
    public VoidCoinsSO Collect;
    [Header("基本属性")]
    public float maxhealth;
    public float currentHealth;
    public int coinsAmount;
    public sword sword;
    public bool over=false;
    public GameObject player;
    [Header("广播")]
    public VoidEventSO gameOverEventSO;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    public float invulnerableCounter;
    public bool invulnerable;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    public UnityEvent<Character> OnHealthChange;
    private void Awake()
    {
        sword = new sword();
    }
    private void OnEnable()
    {
        newGameEventSO.OnEventRaised += NewGame;
        Collect.OnEventRaised += CollectCoins;
        RegisterSaveData();
    }
    private void Start()
    {
     
    }
    private void OnDisable()
    {
        newGameEventSO.OnEventRaised -= NewGame;
        Collect.OnEventRaised -= CollectCoins;
       UnRegisterSaveData();
    }

    private void CollectCoins()
    {
        coinsAmount++;
        
    }

    private void NewGame()
    {
        coinsAmount = 0;
        currentHealth = maxhealth;
        OnHealthChange?.Invoke(this);
        over = false;
        RemoveSword();
    }
    private void Update()
    {
        if (invulnerable&&!over) 
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
        
    }
    public void TakeDamage(Attack attacker)
    {
        
        Debug.Log((int)attacker.GetComponentInParent<Character>().sword.force);
        if (invulnerable)
        {
            return;
        }
        if (currentHealth - attacker.damage- (int)attacker.GetComponentInParent<Character>().sword.force > 0) 
        {
            currentHealth -= attacker.damage ;
            currentHealth -= (int)attacker.GetComponentInParent<Character>().sword.force;
            OnTakeDamage?.Invoke(attacker.transform);
        TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }
    public void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID]=new SerializeVector3(transform.position);
            data.floatSaveData[GetDataID().ID + "health"] = this.currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID + "health", this.currentHealth);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();
            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            OnHealthChange?.Invoke(this);

        }
    }
    private IEnumerator Isave()
    {
        ISaveable saveable = this;
        yield return new WaitForSeconds(0.1f);
        saveable.RegisterSaveData();
    }

    public void RegisterSaveData()
    {
        DataManager.instance.RegisterSaveData(this);
    }

    public void UnRegisterSaveData()
    {
        DataManager.instance.UnRegisterSaveData(this);
    }
    public void RemoveSword()
    {
        sword.RemoveValue(player);
    }
}
