using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class text : MonoBehaviour
{
    public Text t;
    public int amount ;
    public Character character ;
    private void add()
    {
       
        
    }
    public VoidCoinsSO addEvent;
    private void OnEnable()
    {
        addEvent.OnEventRaised += add;
    }


    private void OnDisable()
    {
        addEvent.OnEventRaised -= add;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //amount = character.coinsAmount;
        //t.text = "coins amount:" + amount.ToString();
        amount = character.coinsAmount;
        t.text = "coins amount:" + (character.coinsAmount).ToString();
    }
    
}
