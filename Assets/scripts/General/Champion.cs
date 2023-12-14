using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour, IInteractable
{
    public VoidEventSO GameOverSO;
    public void TriggerAction(GameObject owner)
    {
        GameOverSO.RaiseEvent();
    }
}
