using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bootDrop : MonoBehaviour, IInteractable
{
    public boot myboot=new boot(5.0f,0,0);
    public VoidEventSO GetBootEventSO;
    public void TriggerAction(GameObject owner)
    {

       //ui
        GetBootEventSO.RaiseEvent();
        myboot.Dress(owner);
        Destroy(this.gameObject);
    }
}
