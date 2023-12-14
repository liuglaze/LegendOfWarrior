using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : ArmourBase
{
    public sword(float speed, float force, float health) : base(speed, force, health)
    {

    }
    public sword():base()
    {

    }
    public override void RemoveValue(GameObject owner)
    {
        owner.GetComponent<Character>().sword.force = 0;
    }
}
