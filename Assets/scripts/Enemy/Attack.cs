using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject owner;
    public int damage=10;
    public VoidEventSO getSword;
    public float attackRange;
    public float attackRate;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        
        other.GetComponent<Character>()?.TakeDamage(this);
        
    }
}
