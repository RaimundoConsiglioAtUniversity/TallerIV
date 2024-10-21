using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundCheck : MonoBehaviour
{
    
    //Ground Check Vars
    public bool isGrounded => groundCheck.IsTouchingLayers(groundLayer);
    public Collider2D groundCheck;
    public LayerMask groundLayer;

    public event Action<GameObject> OnGroundActions;
    public event Action<GameObject> OnAirActions;
    
    void Update()
    {
        Check();
    }

    void Check()
    {
        if (isGrounded)
        {
            OnGroundActions(gameObject);
            //player.anim.SetBool("jump", false);
            //player.anim.SetBool("isFalling", false);
        }
        else
        {
            OnAirActions(gameObject);
        }
    }
}
