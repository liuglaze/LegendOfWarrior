using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArmourUI : MonoBehaviour
{
    public VoidEventSO getSwordEventSO;
    public VoidEventSO getBootEventSO;
    private void OnEnable()
    {
        getSwordEventSO.OnEventRaised += OnGetSword;
        getBootEventSO.OnEventRaised += OnGetBoot;
    }
    private void OnDisable()
    {
        getSwordEventSO.OnEventRaised -= OnGetSword;
        getBootEventSO.OnEventRaised -= OnGetBoot;
    }

    private void OnGetBoot()
    {
        boot.SetActive(true);
    }

    private void OnGetSword()
    {
        sword.SetActive(true);
    }

    public GameObject sword;
    public GameObject boot;
}
