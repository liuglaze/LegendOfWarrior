using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEventSO;
    public VoidEventSO loadDataEventSO;
    public VoidEventSO afterSceneLoadEventSO;
    public VoidEventSO backToMenuEventSO;
    public VoidEventSO gameOverEventSO;
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;
    private Character charc;
    private CapsuleCollider2D capsual;
    [Header("基本参数")]
    public GameObject player;
    public float currentSpeed;
    public float Firstspeed;
    public float jumpforce;
    public float hurtForce;
    public float jumpTime = 0.0f;
    public float jumpHoldforce;
    public float lastGroundTime;
    public float grouondBufferTime;
    public float lastWallTime;
    public float wallBuffetTime;
    public bool isJumping;
    public float dashTime;
    public bool isDash=false;
    public float dashForce;
    public bool dash2;
    public bool canJump;
    public bool canDash=false;
    public boot normalBoot;
  
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("状态")]
    public bool isAttack;
    public bool isHurt;
    public bool isDead;
    public bool isCrouch;
    public bool isWall;
    [Header("物理材质")]
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D mocali;
    public PhysicsMaterial2D smooth;
    private void Awake()
    {
        normalBoot = new boot();
        
        capsual=GetComponent<CapsuleCollider2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputControl.Enable();
        //新输入系统实现大小跳
        //inputControl.Gameplay.jump.performed += ctx => 
        //{
        //    if (ctx.interaction is HoldInteraction)
        //    {
        //        rb.AddForce(transform.up * jumpforce*1.5f, ForceMode2D.Impulse);
        //        return;
        //    }
            
        //    else if (ctx.interaction is TapInteraction) { rb.AddForce(transform.up * jumpforce , ForceMode2D.Impulse); }
        //};
       charc = GetComponent<Character>();
        inputControl.Gameplay.attack.started += PlayerAttack;
    }



    private void OnEnable()
    {
        afterSceneLoadEventSO.OnEventRaised += OnAfterSceneLoadedEvent;
        sceneLoadEventSO.LoadRequestEvent += OnLoadEvent;
        loadDataEventSO.OnEventRaised += OnLoadDataEvent;
        backToMenuEventSO.OnEventRaised += OnBackToMenu;
        gameOverEventSO.OnEventRaised += OnGameOver;
        currentSpeed = Firstspeed;
        originalOffset = capsual.offset;
        originalSize=capsual.size;
    }
    private void OnDisable()
    {
        afterSceneLoadEventSO.OnEventRaised -= OnAfterSceneLoadedEvent;
        inputControl.Disable();
        sceneLoadEventSO.LoadRequestEvent -= OnLoadEvent;
        loadDataEventSO.OnEventRaised -= OnLoadDataEvent;
        backToMenuEventSO.OnEventRaised -= OnBackToMenu;
        gameOverEventSO.OnEventRaised -= OnGameOver;
    }

    private void OnGameOver()
    {
        inputControl.Gameplay.Disable();
        canJump = false;
        
    }

    private void OnBackToMenu()
    {
        Time.timeScale = 1;
        isDead = false;
    }

    private void OnLoadDataEvent()
    {
        Time.timeScale = 1;
        isDead = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Enable();
        canJump = true;
        Time.timeScale = 1;
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
        canJump = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(player.transform.position.y<-20)
        {
            gameOverEventSO.RaiseEvent();
        }
        isWall = (physicsCheck.isLeftWall && inputDirection.x < 0) || (physicsCheck.isRightWall && inputDirection.x > 0);
        if (dashTime>Time.time) 
        {
            dash2 = true;
        }
        else dash2 = false;
        CheckState();
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        jump();
        dash();
        lastGroundTime -= Time.deltaTime;
        lastWallTime -= Time.deltaTime;
        if (physicsCheck.isGround)
        {
            lastGroundTime = grouondBufferTime;
        }
        if (isWall)
        {
            lastWallTime = wallBuffetTime;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if(!physicsCheck.isGround&&rb.velocity.y>0) 
        {
          isJumping = true;
        }
        else isJumping = false;
    }
    private void FixedUpdate()
    {
       
        if (!isHurt&&!isAttack&& Time.time > dashTime)
        Move();
    }
   

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * (currentSpeed+normalBoot.speed) , rb.velocity.y);
        //翻转
        int faceDir = (int)transform.localScale.x;
        if(inputDirection.x > 0) { faceDir=1; }
        if (inputDirection.x<0){ faceDir = -1; };
        transform.localScale = new Vector3(faceDir, 1, 1);

        if(inputDirection.y<-0.5f&&physicsCheck.isGround)
        {
            isCrouch = true;
            currentSpeed = Firstspeed / 2;
            capsual.offset = new Vector2(-0.05f, 0.85f);
            capsual.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            isCrouch=false;
            currentSpeed = Firstspeed;
            capsual.offset = originalOffset;
            capsual.size = originalSize;
        }
    }
     
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;

    }

    public void Gethurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    private void CheckState()
    {
        if (isWall)
            capsuleCollider.sharedMaterial = wall;
        else if (physicsCheck.isWall)
            capsuleCollider.sharedMaterial = smooth;
        else
            capsuleCollider.sharedMaterial = mocali;
}
    private void jump()
    {
        if (  canJump &&(!isDead && (Input.GetKeyDown(KeyCode.Space) && ((lastGroundTime > 0 && !isJumping) || lastWallTime > 0))))

        {
            jumpTime = Time.time + 0.2f;
            rb.AddForce(transform.up * (jumpforce+normalBoot.jumpForce), ForceMode2D.Impulse);
            GetComponent<AudioDefination>().PlayAudioClip();
        }
        if (Input.GetKey(KeyCode.Space) && !physicsCheck.isGround && Time.time < jumpTime)
        {
            rb.AddForce(transform.up * jumpHoldforce, ForceMode2D.Impulse);
        }
    }
    public void dash()
    {
        
        if (canDash && ((Input.GetKeyDown(KeyCode.LeftShift)&&!isDash)&&!isDead))
        {
            isDash = true;
            dashTime = Time.time + 0.23f;
            //rb.AddForce( (new Vector2(inputDirection.x*dashForce,0)), ForceMode2D.Impulse);
            rb.velocity = new Vector2(inputDirection.x * currentSpeed * 3, 0);
            rb.gravityScale = 0;
            if (!charc.invulnerable)
            {
                charc.invulnerable = true;
                charc.invulnerableCounter = 0.3f;
            }

        }
        if(Time.time > dashTime&&isDash)
        {
            rb.gravityScale = 4;
            if(physicsCheck.isGround)isDash = false;
        }
    }
     public void RemoveBoot()
    {
        normalBoot.RemoveValue(player);
    }
}
