using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("²ÎÊý")]
    public Vector2 bottomOffset;
    public Vector2 bottomOffset2;
    public Vector2 offset;
    public Vector2 offset2;
    public float checkRadius;
    public float checkRadius2;
    public LayerMask groundLayer;

    [Header("×´Ì¬")]
    public bool isGround;
    public bool isGround2;
    public bool isLeftWall;
    public bool isRightWall;
    public bool isWall;

    private void Update()
    {
        Check();
    }
    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRadius, groundLayer) ||
                 Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset2, checkRadius, groundLayer);
        isGround2 = Physics2D.OverlapCircle( (Vector2)(transform.position) + new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y), checkRadius, groundLayer);
        isLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + offset2, checkRadius2, groundLayer);
        isRightWall = Physics2D.OverlapCircle((Vector2)transform.position + offset, checkRadius2, groundLayer); //||
                 //Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset2, checkRadius, groundLayer);
        //isGround = Physics2D.OverlapBox((Vector2)transform.position , bottomOffset,  groundLayer);
        if (isLeftWall || isRightWall)
        {
            isWall = true;
        }
        else isWall= false;
        

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset,checkRadius);
        Gizmos.DrawWireSphere(transform.localScale * ((Vector2)(transform.position) + bottomOffset) + bottomOffset, checkRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset2, checkRadius);
        //Gizmos.DrawCube((Vector2)transform.position, bottomOffset);
        Gizmos.DrawWireSphere((Vector2)transform.position + offset, checkRadius2);
        Gizmos.DrawWireSphere((Vector2)transform.position + offset2, checkRadius2);
    }
   
    
}
