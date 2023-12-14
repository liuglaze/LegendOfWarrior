using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public VoidCoinsSO Collect;
    private void OnTriggerStay2D(Collider2D other)
    {
        collectRaised();
        Destroy(this.gameObject);
       
    }
    public void collectRaised()
    {
        Collect?.RaiseEvent();
    }
}
