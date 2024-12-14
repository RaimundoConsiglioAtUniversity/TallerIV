using System.Collections;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class FollowAI : MonoBehaviour
{
    public PlayerController pony;
    public PlayerInput player;
    public float nextWaypointDistance = 3;
    public float waypointDistance = 0f;
    public Vector2 waypointDirection = Vector2.zero;
    public float targetDistance = 2f;
    public Vector2 targetDirection = Vector2.left;
    public float targetJumpHeight;
    public Rigidbody2D rb;

    private Path path;
    int currentWaypoint = 0;
    bool reachedPathEnd = false;

    Seeker seeker;

    public float hInput;
    public float vInput;
    public bool tryTapJump = false;
    public bool tryHoldJump = false;
    public bool tryRun = false;

    private float jumpTime = 0f;
    private bool isJumping = false;

    private Transform Target => pony.tribe.target.transform;

    Vector2 frontObstacleHeightDifference;

    string debugStr = "";

Vector2 positionDifference;


    void Awake()
    {
        pony = transform.parent.parent.GetComponent<PlayerController>();
        seeker = GetComponent<Seeker>();
        rb = transform.parent.parent.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerInput.Instance;

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }

    IEnumerator UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, Target.position, OnPathComplete);

        yield return new WaitForSeconds(Time.fixedDeltaTime);

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }
    
    void OnPathComplete(Path p)
    {
        if (p.error)
            return;

        path = p;
        currentWaypoint = 0;
    }

    void Update()
    {
        debugStr = $"Pony: {pony.name}  ";

        if (path == null)
            return;

        if (currentWaypoint < path.vectorPath.Count)
            reachedPathEnd = false;
        
        else
        {
            reachedPathEnd = true;
            return;
        }

        UpdatePathProgress();

        hInput = Mathf.Abs(waypointDirection.x) > 0.2f && Mathf.Abs(targetDistance) > 2f ? Mathf.Sign(waypointDirection.x) : 0f;
        vInput = Mathf.Abs(targetDistance) > 1f ? -1 : 0;
        tryRun = Mathf.Abs(targetDistance) > 2.5f;

        HandleJumpLogic();

        print(debugStr);
    }

    void HandleJumpLogic()
    {
        //debugStr += "Entered HandleJumpLogic\n";
        
        CalcJumpHeight();
            jumpTime += Time.deltaTime;

        if (ShouldJump())
        {
            //debugStr += "Should Jump Returned True\n";
            if (!isJumping)
            {
                tryTapJump = true;
                isJumping = true;
                jumpTime = 0f;
            }
            else
            {
                float holdJumpDuration = Mathf.Clamp(targetJumpHeight/(3.9f/pony.stats.maxJumpTime), 0.01f, pony.stats.maxJumpTime);
                float f = Mathf.Pow(holdJumpDuration, 1f/(pony.stats.maxJumpTime * 0.8f));
                
                if (targetJumpHeight > 3.5)
                    f += 0.2f;
                
                else if (targetJumpHeight > 3)
                    f += 0.1f;
                

                debugStr += $"Hold Jump Duration: {f}\n";
                StartCoroutine(HoldJump(f));
            }
        }
        else
        {
            //debugStr += "Should Jump Returned False\n";
            jumpTime = 0f;
            isJumping = false;
            tryTapJump = false;
        }
            //debugStr += $"Jump Time: {jumpTime}\n";
    }

    private IEnumerator HoldJump(float duration)
    {
        tryHoldJump = true;

        yield return new WaitForSeconds(duration);

        tryHoldJump = false;
    }

    float CalcJumpHeight()
    {
        RaycastHit2D obstacleHit = Physics2D.Raycast(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), rb.position.y + 10f), Vector2.down, 11f, pony.stats.obstacleLayers);
        Debug.DrawRay(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), rb.position.y + 10f), Vector2.down * 11f, Color.red);

        if (pony.groundC.IsGrounded || (!pony.groundC.IsGrounded && pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps)))
        {
            frontObstacleHeightDifference = obstacleHit.point - rb.position;
            positionDifference = (Vector2)Target.position - rb.position;
        }

        
        if (positionDifference.y > 0.2f)
            targetJumpHeight =  Mathf.Abs(positionDifference.y);
        

        else if (obstacleHit.collider != null && positionDifference.y < 0.1f && Mathf.Abs(positionDifference.x) > 1.2f)
        {
                //debugStr += $"frontObstacleHeightDifference {frontObstacleHeightDifference}\n";
                targetJumpHeight = Mathf.Abs(frontObstacleHeightDifference.y) + 1f;
        }
        
        //debugStr += $"positionDifference {positionDifference}\n";
        debugStr += $"targetJumpHeight {targetJumpHeight}\n";

        return targetJumpHeight;
    }

    bool ShouldJump()
    {
        // Raycast to check for obstacles in front
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * Mathf.Sign(waypointDirection.x), 1f, pony.stats.obstacleLayers);
        Debug.DrawRay(rb.position, Mathf.Sign(waypointDirection.x) * 1f * Vector2.right, Color.magenta);

        
        if (hit.collider == null)
            return false;
        


        if (pony.groundC.IsGrounded)
        {
            //debugStr += $"pony.groundC.IsGrounded: {pony.groundC.IsGrounded}\n";
            return true;
        }
            
        else if (!pony.groundC.IsGrounded && pony.rb.velocity.y < 0.01f)
        {
            //debugStr += $"pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps) : {pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps)}\n";
            return pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps);
        }
        
        return false;
    }

    private void UpdatePathProgress()
    {
        Vector2 CurrentWaypointPath = (Vector2)path.vectorPath[currentWaypoint];

        if ((CurrentWaypointPath - rb.position).normalized.magnitude > 0.1f)
            waypointDirection = (CurrentWaypointPath - rb.position).normalized;

        if (((Vector2)Target.position - rb.position).normalized.magnitude > 0.1f)
            targetDirection = ((Vector2)Target.position - rb.position).normalized;

        waypointDistance = Vector2.Distance(rb.position, CurrentWaypointPath);
        targetDistance = Vector2.Distance(rb.position, Target.position);

        if (waypointDistance < 0.1f)
            waypointDistance = 0.1f;

        if (waypointDistance < nextWaypointDistance)
            currentWaypoint++;
        
    }
}
