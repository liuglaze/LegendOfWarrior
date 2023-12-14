using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck pCheck;
    private PlayerController controller;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pCheck = rb.GetComponent<PhysicsCheck>();
        controller = rb.GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetBool("isCrouch", controller.isCrouch);
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", pCheck.isGround);
        anim.SetBool("isDead", controller.isDead);
        anim.SetBool("isAttack",controller.isAttack);
        anim.SetBool("dash", controller.dash2);
        anim.SetBool("isWall", controller.isWall);
    }
    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }
    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
}
