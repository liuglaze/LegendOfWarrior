using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class swordDrop : MonoBehaviour,IInteractable
{
    public VoidEventSO getArmour;
    public sword sword = new sword(0, 50, 0);
    public void TriggerAction(GameObject owner)
    {
        owner.GetComponent<Character>().sword = this.sword;
        owner.GetComponent<Transform>().Find("Attack Area").localScale = new Vector3(2, 1, 1);
        Debug.Log(owner.GetComponent<Character>().sword.force);
        getArmour.RaiseEvent();
        Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}
