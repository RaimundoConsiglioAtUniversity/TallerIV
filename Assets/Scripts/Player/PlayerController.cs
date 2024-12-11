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
    public float coyoteTimeCounter {get; private set; }


    public FriendInput inputAI;
    public FollowAI logicAI;
    public BaseInput inputController;
    public bool isDucking => vInput.SteppedValue == 1 && groundCheck.isGrounded;
    public bool isJumping => inputController.pressedJump;
    public bool isHoldingJump => inputController.heldJump;
    public bool isRunning => inputController.pressedRun;
    public InputAxis hInput = new(0f, 0.2f);
    public InputAxis vInput = new(0f, 0.2f);


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

    void Awake()
    {
        inputAI = GetComponent<FriendInput>();
        logicAI = GetComponent<FollowAI>();
    }

    void Start()
    {
        if (PlayerInput.Instance.activePony == this)
        {
            inputController = PlayerInput.Instance;
            inputAI.enabled = false;
            logicAI.enabled = false;
        }
        else
        {
            inputController = inputAI;
            inputAI.enabled = true;
            logicAI.enabled = true;
        }

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
        hInput.value = inputController.hInput;
        vInput.value = inputController.vInput;

        Accelerate();
        charRB.velocity = new(moveSpeed, charRB.velocity.y);
    }

    void ActionInput()
    {
        if (inputController.pressedAction1)
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
        if (isJumping)
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
            if (isHoldingJump)
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
            if (hInput.Pressed)
            {
                if (isDucking)
                    return duckMultiplier * walkSpeed * hInput.Sign;
            
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
            accel += accelPow / 2;

        speedChange = Mathf.Clamp(Mathf.Pow(difference, accel), minAccel, 10000f);
        moveSpeed += speedChange * Time.deltaTime * sign;
    }

    void OnGroundActions(GameObject gO)
    {
        if(gO != gameObject)
            return;


        ResetCoyoteTimer();
        ResetAirJumps();
    }

    void OnAirActions(GameObject gO)
    {
        if(gO != gameObject)
            return;


        DecrementCoyoteTimer();
    }
}
