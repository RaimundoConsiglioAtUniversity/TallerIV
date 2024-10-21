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
    public float flapStrength;
    public float upGravity;
    public float downGravity;

    public float coyoteTimeS = 0.05f;
    float coyoteTimeCounter;

    public float jumpBufferTimeS = 0.05f;
    float jumpBufferCounter;

    public int currentAirJumps = 0;
    public int maxAirJumps = 6;
    public bool canFly;
    public bool canThunder;
    public bool canTeleport;
    public bool canTK;
    public bool canGroundPound;
    public bool canPlant;

    //Walk-Run Related Vars
    public bool isRunning => Input.GetButton("Run");
    public float hAxis =>  Input.GetAxis("Horizontal");
    public Axis hInput = new(0f, 0.2f);
    public float vAxis => Input.GetAxis("Vertical");
    public Axis vInput = new(0f, 0.2f);
    public float accelPow = 2f;
    public float minAccel = 3f;
    [SerializeField] private float speedChange;
    [SerializeField] private float moveSpeed = 0f;
    
    [SerializeField] private float currentTargetSpeed = 0f;

    public bool isLookingRight;
    public Rigidbody2D charRB;
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
        currentTargetSpeed = targetMoveSpeed;
        WalkInput();
        JumpInput();
        
        if (moveSpeed > 0.1f)
            OnLookingRight();
        
        if (moveSpeed < -0.1f)
            OnLookingLeft();

        charRB.ClampVelocity(30f, 50f);
    }

    void OnLookingRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void OnLookingLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void WalkInput()
    {
        hInput.value = hAxis;
        vInput.value = vAxis;

        Accelerate();
        charRB.velocity = new(moveSpeed, charRB.velocity.y);
    }

    void ActionInput()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (canTeleport)
            {
                //getMopusePosition
                //Create ray from pony to it
                //Check if any nearby point is not ground
                // Move Pony there
            }
            if (canTK)
            {
                //getMopusePosition
                //Create ray from pony to it
                //Check if any nearby point has movable object
                //select object and follow cursor until reclicked
            }
            if (canPlant)
            {
                // if near a plant, interact with it
            }
            if (canThunder)
            {
                //getMopusePosition
                //Summon from same x posuition until it hits ground
            }
        }
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

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
                DoJump(jumpStrength);
            else if (canFly && currentAirJumps < maxAirJumps)
            {
                DoJump(flapStrength);
                currentAirJumps++;
            }
        }

        //Changes Gravity for Tighter Jump Arc, and allows Short & High Jumps
        if (charRB.velocity.y > 0)
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

    void DoJump(float jumpStrength)
    {
        //audioS.PlayOneShot(jumpSFX);
        charRB.velocity = new Vector2(charRB.velocity.x, 0);
        charRB.velocity = new Vector2(charRB.velocity.x, jumpStrength);

        jumpBufferCounter = 0f;
    }
    void ResetAirJumps() => currentAirJumps = 0;
    void ResetCoyoteTimer() => coyoteTimeCounter = coyoteTimeS;
    void DecrementCoyoteTimer() => coyoteTimeCounter -= Time.deltaTime;

    float targetMoveSpeed
    {
        get
        {
            if (hInput.IsPressed)
            {
                if (vInput.SteppedValue == 1 && groundCheck.isGrounded)
                    return duckMultiplier * walkSpeed * hInput.Sign;
                else if (vInput.IsPressed && vInput.Sign == -1 && !groundCheck.isGrounded && canGroundPound)
                {
                        DoGroundPound();
                    return 0;
                }
            
                else if (isRunning)
                    return runMultiplier * walkSpeed * hInput.Sign;

                else
                    return walkSpeed * hInput.Sign;
            }

            else
                return 0f;
        }
    }

    void DoGroundPound()
    {
        charRB.velocity = new Vector2(0f, -30f);
    }
    void Accelerate()
    {
        float difference = Mathf.Abs(targetMoveSpeed - moveSpeed);
        int sign = (int)Mathf.Sign(targetMoveSpeed - moveSpeed);
        float accel = accelPow;

        if (difference <= 0.5f)
        {
            moveSpeed = targetMoveSpeed;
            return;
        }

        if (targetMoveSpeed == 0f)
            accel += (accelPow / 2);

        speedChange = Mathf.Clamp(Mathf.Pow(difference, accel), minAccel, 10000f);
        moveSpeed += speedChange * Time.deltaTime * sign;
    }

    void OnGroundActions(GameObject gO)
    {
        if(gO != gameObject)
            return;


        print($"{gameObject.name} Ground!");

        ResetCoyoteTimer();
        ResetAirJumps();
    }

    void OnAirActions(GameObject gO)
    {
        if(gO != gameObject)
            return;

        print($"{gameObject.name} Not Ground!");

        DecrementCoyoteTimer();
    }
}
