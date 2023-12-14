using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    public PlayerInputControl playerInput;
    public Transform playerTrans;
    public Animator anim;
    public GameObject signSprite;
    public IInteractable targetItem;
    public bool canPress;
    public GameObject owner;
    private void Awake()
    {
        anim=signSprite.GetComponent<Animator>();
        playerInput=new PlayerInputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }
    private void OnDisable()
    {
        canPress = false;
    }
    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale= playerTrans.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress)
        {
            targetItem?.TriggerAction(owner);
            canPress=false;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("interactable"))
        {
            canPress = true;
            targetItem=other.GetComponent<IInteractable>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress=false;
    }
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange==InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device; 
            switch(d.device)
            {
                case Keyboard:
                    anim.Play("keyboard");
                    break;
                case DualShockGamepad:
                    anim.Play("ps");
                    break;
                        
            }
        }
    }
}
