using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    public FriendInput inputAI;
    public FollowAI logicAI;
    public BaseInput inputController;
    public PonyType tribe;
    public Rigidbody2D rb;
    public GroundCheck groundC;


    public bool isDucking => vInput.SteppedValue == 1 && groundC.IsGrounded;
    public bool isJumping => inputController.pressedJump;
    public bool isHoldingJump => inputController.heldJump;
    public bool isRunning => inputController.pressedRun;
    public InputAxis hInput = new(0f, 0.2f);
    public InputAxis vInput = new(0f, 0.2f);


    public float coyoteTimeCounter { get; private set; }
    float jumpBufferCounter;
    public int currentFlaps = 0;


    [SerializeField] private float speedChange;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float currentTargetSpeed = 0f;
    
    [SerializeField] private float TargetMoveSpeed
    {
        get
        {
            float h = hInput.Pressed ? 1f : 0f;
            float r = isRunning ? stats.runMultiplier : 1f;
            float d = isDucking ? stats.duckMultiplier : 1f;

            //print($"Name: {gameObject.name}\nH: {h}\nR: {r}\nD: {d}\nWalkSpeed: {stats.walkSpeed}\nSign: {hInput.Sign}");
            return h * r * d * stats.walkSpeed * hInput.Sign;
        }
    }


    private void OnEnable()
    {
        groundC.OnGroundActions += OnGroundActions;
        groundC.OnAirActions += OnAirActions;
    }

    private void OnDisable()
    {
        groundC.OnGroundActions -= OnGroundActions;
        groundC.OnAirActions -= OnAirActions;
    }

    public void OnEnableAI()
    {
        tribe.OnEnableAI();
    }
    public void OnDisableAI()
    {
        tribe.OnDisableAI();
    }

    void Awake()
    {
        inputAI = GetComponentInChildren<FriendInput>();
        logicAI = GetComponentInChildren<FollowAI>();
        groundC = GetComponentInChildren<GroundCheck>();
        tribe = GetComponentInChildren<PonyType>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        stats = PlayerStats.Instance;
    }

    void Update()
    {
        currentTargetSpeed = TargetMoveSpeed;
        WalkInput();
        JumpInput();
        DoActions();
        
        if (moveSpeed > 0.1f)
            OnLookingRight();
        
        else if (moveSpeed < -0.1f)
            OnLookingLeft();

        ApplyCustomGravity();
        rb.ClampVelocity(30f, 50f);
    }


    void OnLookingRight() => transform.localScale = new Vector3(1, 1, 1);
    void OnLookingLeft() => transform.localScale = new Vector3(-1, 1, 1);

    void WalkInput()
    {
        hInput.value = inputController.hInput;
        vInput.value = inputController.vInput;

        Accelerate();
        rb.velocity = new(moveSpeed, rb.velocity.y);
    }

    void JumpInput()
    {
        if (isJumping)
            jumpBufferCounter = stats.jumpBufferTimeS;
        
        else
            jumpBufferCounter -= Time.deltaTime;
        

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
                DoJump(stats.jumpStrength);
            
            else if (tribe is PonyPegasus && currentFlaps < stats.maxFlaps)
            {
                DoJump(stats.flapStrength);
                currentFlaps++;
            }
        }
    }

    public void DoJump(float jumpStrength)
    {
        //audioS.PlayOneShot(jumpSFX);
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);

        jumpBufferCounter = 0f;
    }

    //Changes Gravity for Tighter Jump Arc, and allows Short & High Jumps
    void ApplyCustomGravity()
    {
        float gravityScale;
        if (rb.velocity.y > 0)
        {
            if (isHoldingJump)
            {
                gravityScale = stats.upGravity;
            }
            else
            {
                gravityScale = stats.upGravity * 2f;
                coyoteTimeCounter = 0f;
            }
        }
        else
        {
            gravityScale = stats.downGravity;
        }

        rb.AddForce(gravityScale * Time.deltaTime * Vector2.down, ForceMode2D.Force);
    }
    
    void DoActions()
    {
        if (inputController.pressedAction1)
            tribe.Action1();
        
        if (inputController.pressedAction2)
            tribe.Action2();
    }

    void ResetAirJumps() => currentFlaps = 0;
    void ResetCoyoteTimer() => coyoteTimeCounter = stats.coyoteTimeS;
    void DecrementCoyoteTimer() => coyoteTimeCounter -= Time.deltaTime;

    void Accelerate()
    {
        float difference = Mathf.Abs(TargetMoveSpeed - moveSpeed);
        int sign = (int)Mathf.Sign(TargetMoveSpeed - moveSpeed);
        float accel = stats.accelPow;

        if (difference <= 0.5f)
        {
            moveSpeed = TargetMoveSpeed;
            return;
        }

        if (TargetMoveSpeed == 0f)
            accel += stats.accelPow / 2;

        speedChange = Mathf.Clamp(Mathf.Pow(difference, accel), stats.minAccel, 10000f);
        moveSpeed += speedChange * Time.deltaTime * sign;
    }

    void OnGroundActions(GameObject gO)
    {
        if (gO != gameObject)
            return;


        ResetCoyoteTimer();
        ResetAirJumps();
    }

    void OnAirActions(GameObject gO)
    {
        if (gO != gameObject)
            return;


        DecrementCoyoteTimer();
    }
}
