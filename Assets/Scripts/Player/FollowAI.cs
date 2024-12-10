using System.Collections;
using UnityEngine;
using Pathfinding;

public class FollowAI : MonoBehaviour
{
    public PlayerController thisPony;
    public PlayerInput player;
    public Transform target;
    public float nextWaypointDistance = 3;
    public float waypointDistance = 0f;
    public Vector2 waypointDirection = Vector2.zero;
    public float targetDistance = 2f;
    public Vector2 targetDirection = Vector2.left;

    private Path path;
    int currentWaypoint = 0;
    bool reachedPathEnd = false;

    Seeker seeker;
    Rigidbody2D rb;


    public float hInput;
    public float vInput;
    public bool tryTapJump = false;
    public bool tryHoldJump = false;
    public bool tryRun = false;

    public float jumpCooldown = 0.1f;
    public float jumpCooldownTimer = 0f;

    void IncrementJumpTimer() => jumpCooldownTimer += Time.deltaTime;
    void ResetJumpTimer() => jumpCooldownTimer = 0f;

    void Awake()
    {
        thisPony = GetComponent<PlayerController>();

        // if (thisPony == player.activePony)
        // {
        //     thisPony.enabled = false;
        // }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerInput.Instance;

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }

    IEnumerator UpdatePath()
    {
        print("Entered Update Path Method");
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

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
        vInput = player.activePony.isDucking ? -1 : 0;
        tryRun = player.activePony.isRunning;

        if (thisPony.groundCheck.isGrounded)
            IncrementJumpTimer();
        else
            ResetJumpTimer();

        if (jumpCooldownTimer > jumpCooldown || !thisPony.groundCheck.isGrounded)
        {
            if ((tryTapJump || tryHoldJump) && target.position.y - rb.position.y > 1f)
            {
                tryHoldJump = true;
                tryTapJump = false;
            }
            else if (!tryTapJump && !tryHoldJump && target.position.y - rb.position.y > 0.8f && rb.velocity.y < 0.01f)
            {
                tryHoldJump = false;
                tryTapJump = true;
            }
        }

        else
        {
            tryHoldJump = false;
            tryTapJump = false;
        }
    }

    private void UpdatePathProgress()
    {
        Vector2 CurrentWaypointPath = (Vector2)path.vectorPath[currentWaypoint];

        if ((CurrentWaypointPath - rb.position).normalized.magnitude > 0.1f)
            waypointDirection = (CurrentWaypointPath - rb.position).normalized;

        if (((Vector2)target.position - rb.position).normalized.magnitude > 0.1f)
            targetDirection = ((Vector2)target.position - rb.position).normalized;

        waypointDistance = Vector2.Distance(rb.position, CurrentWaypointPath);
        targetDistance = Vector2.Distance(rb.position, target.position);

        if (waypointDistance < 0.1f)
            waypointDistance = 0.1f;


        if (waypointDistance < nextWaypointDistance)
        {
            currentWaypoint++;
            print("Incremented Waypoint");
        }
    }
}
