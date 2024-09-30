using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Walking Vars
    public float walkSpeed;
    public float runMultiplier;
    public float duckMultiplier;

    //Jumping Vars
    public float jumpStrength;
    public float upGravity;
    public float downGravity;

    public float coyoteTimeS = 0.05f;
    float coyoteTimeCounter;

    public float jumpBufferTimeS = 0.05f;
    float jumpBufferCounter;

    //Walk-Run Related Vars
    public bool isRunning => Input.GetButton("Run");

    public float hInput =>  Input.GetAxis("Horizontal");
    public float vInput => Input.GetAxis("Vertical");

    public float hDeadZone, vDeadZone;
    public bool isLookingRight;
    public Rigidbody2D character;
    public GroundCheck groundCheck;


    private void OnEnable()
    {
        groundCheck.OnGroundActions += OnGroundActions;
        groundCheck.OnAirActions += OnAirActions;
    }

    private void OnDisable()
    {
        groundCheck.OnGroundActions -= OnGroundActions;
        groundCheck.OnAirActions -= OnAirActions;
    }

    void Start()
    {
        
    }

    void Update()
    {
        WalkInput();
        JumpInput();

        character.ClampVelocity(30f, 50f);
    }

    void WalkInput()
    {
        character.velocity = new(moveSpeed * hInput, character.velocity.y);
    }

    void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTimeS;
            print("Jump!");
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            //audioS.PlayOneShot(jumpSFX);
            character.velocity = new Vector2(character.velocity.x, 0);
            character.velocity = new Vector2(character.velocity.x, jumpStrength);

            jumpBufferCounter = 0f;
        }

        //Changes Gravity for Tighter Jump Arc, and allows Short & High Jumps
        if (character.velocity.y > 0)
        {
            if (Input.GetButton("Jump"))
                Physics2D.gravity = new Vector2(Physics2D.gravity.x, -upGravity);
            else
            {
                Physics2D.gravity = new Vector2(Physics2D.gravity.x, -upGravity * 2);
                coyoteTimeCounter = 0f;
            }
        }
        else
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, -downGravity);
    }

    void ResetCoyoteTimer() => coyoteTimeCounter = coyoteTimeS;
    void DecrementCoyoteTimer() => coyoteTimeCounter -= Time.deltaTime;

    float moveSpeed
    {
        get
        {
            if (Mathf.RoundToInt(Mathf.Abs(vInput)) == 1 && groundCheck.isGrounded)
                return duckMultiplier * walkSpeed;
            
            else if (isRunning)
                return runMultiplier * walkSpeed;

            else
                return walkSpeed;
        }
    }

    void OnGroundActions(GameObject gO)
    {
        if(gO != gameObject)
            return;


        print($"{gameObject.name} Ground!");

        ResetCoyoteTimer();
    }

    void OnAirActions(GameObject gO)
    {
        if(gO != gameObject)
            return;

        print($"{gameObject.name} Not Ground!");

        DecrementCoyoteTimer();
    }
}
