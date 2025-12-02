using System.Collections;
using UnityEngine;
using Pathfinding;

public class FollowAI : MonoBehaviour
{
    private static readonly WaitForSeconds pathRefreshTime = new(0.5f);
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

    Seeker seeker;

    public float hInput;
    public float vInput;
    public bool tryTapJump = false;
    public bool tryHoldJump = false;
    public bool tryRun = false;

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
        while(true)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, Target.position, OnPathComplete);

            yield return pathRefreshTime;
        }
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

        if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
            return;


        UpdatePathProgress();

        hInput = Mathf.Abs(waypointDirection.x) > 0.2f && targetDistance > 3f ? Mathf.Sign(waypointDirection.x) : 0f;
        //vInput = Mathf.Abs(targetSqrDistance) > 1.2f ? -1 : 0;
        tryRun = Mathf.Abs(targetDistance) > 2f;

        HandleJumpLogic();

        // print(debugStr);
    }

    void HandleJumpLogic()
    {
        // debugStr += "Entered HandleJumpLogic\n";
        
        CalcJumpHeight();

        if (ShouldJump())
        {
            // debugStr += "Should Jump Returned True\n";
            if (!isJumping)
            {
                tryTapJump = true;
                isJumping = true;
            }
            else
            {
                float holdJumpDuration = Mathf.Clamp(targetJumpHeight/(3.9f/pony.stats.maxJumpTime), 0.01f, pony.stats.maxJumpTime);
                float f = Mathf.Pow(holdJumpDuration, 1f/(pony.stats.maxJumpTime * 0.8f));

                if (targetJumpHeight > 3.5)
                    f += 0.2f;
                
                else if (targetJumpHeight > 3)
                    f += 0.1f;
                

                // debugStr += $"Hold Jump Duration: {f}\n";
                StartCoroutine(HoldJump(f));
            }
        }
        else
        {
            // debugStr += "Should Jump Returned False\n";
            isJumping = false;
            tryTapJump = false;
        }
    }

    private IEnumerator HoldJump(float duration)
    {
        tryHoldJump = true;

        yield return new WaitForSeconds(duration);

        tryHoldJump = false;
    }

    float CalcJumpHeight()
    {
        RaycastHit2D obstacleHit = Physics2D.Raycast(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), rb.position.y + 4.5f), Vector2.down, 11f, pony.stats.obstacleLayers);
        Debug.DrawRay(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), rb.position.y + 4.5f), Vector2.down * 5f, Color.red);


        if (obstacleHit.collider == null)
            return targetJumpHeight;

        if (pony.IsGrounded || (!pony.IsGrounded && pony.tribe is PonyPegasus && pony.HasFlapsLeft))
        {
            frontObstacleHeightDifference = obstacleHit.point - rb.position;
            positionDifference = (Vector2)Target.position - rb.position;
        }

        
        if (positionDifference.y > 0.2f)
            targetJumpHeight =  Mathf.Abs(positionDifference.y);
        

        else if (positionDifference.y < 0.1f && Mathf.Abs(positionDifference.x) > 1.2f)
        {
                debugStr += $"frontObstacleHeightDifference {frontObstacleHeightDifference}\n";
                targetJumpHeight = Mathf.Abs(frontObstacleHeightDifference.y) + 1f; //???
        }
        
        // debugStr += $"positionDifference {positionDifference}\n";
        // debugStr += $"targetJumpHeight {targetJumpHeight}\n";

        return targetJumpHeight;
    }

    bool ShouldJump()
    {

        float dir = Mathf.Sign(waypointDirection.x);
        
        if (dir == 0)
            dir = Mathf.Sign(targetDirection.x); // To Avoid Jank
        
        // Raycast to check for obstacles in front
        Vector2 rayDir = Vector2.right * dir;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rayDir, 1f, pony.stats.obstacleLayers);
        Debug.DrawRay(rb.position, rayDir, Color.magenta);

        
        if (hit.collider == null)
            return false;
        


        if (pony.IsGrounded)
        {
            // debugStr += $"pony.groundC.IsGrounded: {pony.groundC.IsGrounded}\n";
            return true;
        }
            
        else if (!pony.IsGrounded && pony.rb.linearVelocity.y < 0.01f)
        {
            // debugStr += $"pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps) : {pony.tribe is PonyPegasus && (pony.currentFlaps < pony.stats.maxFlaps)}\n";
            return pony.tribe is PonyPegasus && pony.HasFlapsLeft;
        }
        
        return false;
    }

    private void UpdatePathProgress()
    {
        currentWaypoint = Mathf.Clamp(currentWaypoint, 0, path.vectorPath.Count - 1);

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
