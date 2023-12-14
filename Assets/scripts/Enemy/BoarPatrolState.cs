using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    //巡逻函数要操作enemy，于是要留一个外部接口函数来传入enemy来操作。
    //而current enemy方便给其他函数操作enemy
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy=enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if ((!currentEnemy.physicsCheck.isGround2 ) || ((currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.isRightWall) && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);

        }
        else currentEnemy.anim.SetBool("walk", true);
    }


    public override void PhysicsUpdate()
    {
     
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);

    }
   
}
