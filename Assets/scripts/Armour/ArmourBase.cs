using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBase 
{
    public float speed=0;
    public float force=0;
    public float health = 0;
   public ArmourBase() 
    { 

    }
    public ArmourBase(float speed, float force, float health)
    {
        this.speed = speed;
        this.force = force;
        this.health = health;
    }
    virtual public void Dress(GameObject owner)
    {

    }
    virtual public void RemoveValue(GameObject owner)
    {

    }
}
