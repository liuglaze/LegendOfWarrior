using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class boot : ArmourBase
{
    public float jumpForce=5;
   public boot(float speed, float force, float health) :base( speed,  force,  health)
    {

    }
    public boot():base()
    {

    }
    public override void Dress(GameObject owner)
    {
        owner.GetComponent<PlayerController>().canDash = true;
        owner.GetComponent<PlayerController>().normalBoot = this;
    }
    public override void RemoveValue(GameObject owner)
    {
        owner.GetComponent<PlayerController>().normalBoot.speed = 0;
        owner.GetComponent<PlayerController>().canDash=false;
    }
}
