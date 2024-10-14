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

        charRB.ClampVelocity(30f, 50f);
    }

    void WalkInput()
    {
        hInput.value = hAxis;
        vInput.value = vAxis;

        Accelerate();
        charRB.velocity = new(moveSpeed, charRB.velocity.y);
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
            charRB.velocity = new Vector2(charRB.velocity.x, 0);
            charRB.velocity = new Vector2(charRB.velocity.x, jumpStrength);

            jumpBufferCounter = 0f;
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
            
                else if (isRunning)
                    return runMultiplier * walkSpeed * hInput.Sign;

                else
                    return walkSpeed * hInput.Sign;
            }

            else
                return 0f;
        }
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
    }

    void OnAirActions(GameObject gO)
    {
        if(gO != gameObject)
            return;

        print($"{gameObject.name} Not Ground!");

        DecrementCoyoteTimer();
    }
}
