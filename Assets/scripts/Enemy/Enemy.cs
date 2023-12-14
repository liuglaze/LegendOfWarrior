using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public  Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public float force;
    public bool isHurt;
    public bool isDead;
    public bool wait;
    public BaseState patrolState;
    public BaseState currentState;
    public BaseState chaseState;
    public Transform attackerTrans;
    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public float lostTime;
    public float lostTimeCounter;
    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    private void OnEnable()
    {

        currentState = patrolState;
        currentState.OnEnter(this);
    }
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        //waitTimeCounter = waitTime;
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
            counter();
    }
    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait)
            Move();
        currentState.PhysicsUpdate();
    }
    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {
        
        rb.velocity=new Vector2(currentSpeed*faceDir.x,rb.velocity.y);
    }
    public void counter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter < 0||isHurt)
            {
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                waitTimeCounter = waitTime;
                anim.SetBool("walk", true);
                wait = false;
            }
        }
        if(!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter-=Time.deltaTime;
        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null

        };
        currentState.OnExit();
        currentState=newState;
        currentState.OnEnter(this);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0,0), 0.2f);
    }
    #region event
    public void Die()
    {
        gameObject.layer = 2;
        isDead = true;
        anim.SetBool("dead", true);
    }
    public void Destroyboar()
    {
        Destroy(this.gameObject);
    }
    public void Hurt(Transform attacker)
    {
        anim.SetTrigger("hurt");
        attackerTrans = attacker;
        Vector2 dir = new Vector2(transform.position.x - attackerTrans.position.x, 0).normalized;
        if(attackerTrans.position.x-transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        if (attackerTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);    
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        StartCoroutine(onHurt(dir));
       
    }
    private IEnumerator onHurt(Vector2 dir)
    {
        isHurt = true;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        isHurt = false;
    }
    #endregion
}
