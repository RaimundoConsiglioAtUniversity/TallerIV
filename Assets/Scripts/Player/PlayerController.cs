using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    public FriendInput inputAI;
    public FollowAI logicAI;
    public BaseInput inputController;
    public PonyType tribe;
    public Rigidbody2D rb;
    public Collider2D col;
    public GroundCheck groundC;
    public PonyAnim anim;
    public PonySpriteReference sprite;


    public bool isDucking => vInput.SteppedValue == 1 && IsGrounded;
    public bool isJumping => inputController.pressedJump;
    public bool isHoldingJump => inputController.heldJump;
    public bool isRunning => inputController.pressedRun;
    public InputAxis hInput = new(0f, 0.2f);
    public InputAxis vInput = new(0f, 0.2f);

    public bool HasFlapsLeft => currentFlaps < stats.maxFlaps;
    public bool IsGrounded => groundC.IsGrounded;

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
        sprite.renderer2D.material = stats.defaultMaterial;
        tribe.OnEnableAI();
    }
    public void OnDisableAI()
    {
        sprite.renderer2D.material = stats.outlineMaterial;
        tribe.OnDisableAI();
    }

    void Awake()
    {
        inputAI = GetComponentInChildren<FriendInput>();
        logicAI = GetComponentInChildren<FollowAI>();
        groundC = GetComponentInChildren<GroundCheck>();
        tribe = GetComponentInChildren<PonyType>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<PonyAnim>();
        sprite = GetComponentInChildren<PonySpriteReference>();
    }

    void Start()
    {
        stats = PlayerStats.Instance;
    }

    void Update()
    {
        CheckAnimationState();
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
        rb.linearVelocity = new(moveSpeed, rb.linearVelocity.y);
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
                DoJump(stats.jumpStrength, Animations.Jump_Rise);
            
            else if (tribe is PonyPegasus && HasFlapsLeft)
            {
                DoJump(stats.flapStrength, Animations.Flap);
                currentFlaps++;
            }
        }
    }

    public void DoJump(float jumpStrength, Animations jumpType)
    {
        //audioS.PlayOneShot(jumpSFX);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);
        anim.Play(jumpType);

        jumpBufferCounter = 0f;
    }

    //Changes Gravity for Tighter Jump Arc, and allows Short & High Jumps
    void ApplyCustomGravity()
    {
        float gravityScale;
        if (rb.linearVelocity.y > 0)
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

    void CheckAnimationState()
    {
        float yAbsoluteVelocity = Mathf.Abs(rb.linearVelocity.y);

        if (IsGrounded && yAbsoluteVelocity < 0.1f)
        {
            float speed = Mathf.Abs(moveSpeed);

            if (speed >= stats.walkSpeed * stats.runMultiplier * 0.98f)
            {
                anim.Play(Animations.Gallop);
            }
            else if (speed >= stats.walkSpeed * 1.1f)
            {
                anim.Play(Animations.Trot);
            }
            else if (speed >= 0.1f)
            {
                anim.Play(Animations.Walk);
            }
            else if (isDucking)
                anim.Play(Animations.Duck);
            else
                anim.Play(Animations.Idle);

        }
        else
        {
            if (yAbsoluteVelocity < 1.5f)
                anim.Play(Animations.Jump_Peak);

            else if (rb.linearVelocity.y <= -1.5f)
                anim.Play(Animations.Fall);

        }
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
