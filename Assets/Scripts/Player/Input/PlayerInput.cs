using System.Linq;
using UnityEngine;

public class PlayerInput : BaseInput
{
    public override bool pressedRun => Input.GetButton("Run");
    public override float hInput =>  Input.GetAxis("Horizontal");
    public override float vInput => Input.GetAxis("Vertical");
    public override bool pressedJump => Input.GetButtonDown("Jump");
    public override bool heldJump => Input.GetButton("Jump");
    public override bool pressedAction1 => Input.GetButtonDown("Action1");
    public override bool pressedAction2 => Input.GetButtonDown("Action2");


    public PlayerController pony;
    public PlayerController[] ponies;
    private int ponyIdx = 0;
    private int PonyIdx
    {
        get => ponyIdx;
        set => ponyIdx = value % ponies.Length;
    }

    public static PlayerInput Instance => instance;
    private static PlayerInput instance;

    public bool StayToggle { get; private set; }= false;
    public Collider2D screenTrigger;

    public bool withinBounds = true;

    private int levelAreaOverlapCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger collider is one of the level ones.
        if (Screen.levelArea.Contains(other))
        {
            levelAreaOverlapCount++;
            withinBounds = true; // At least one level area collider is overlapping.
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (Screen.levelArea.Contains(other))
        {
            levelAreaOverlapCount--;
            withinBounds = levelAreaOverlapCount > 0; // Only set withinBounds to false if no level area colliders remain.
        }
    }

    void Awake ()
    {
        if (instance != null)
            Destroy(this);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (pony == null)
            pony = ponies[0];

    }

    void Start() => ChangePony(pony);

    void Update ()
    {
        ChangePonyInput();
        
        if (Input.GetButtonDown("StayToggle"))
            StayToggle = !StayToggle;
    
    }

    private void ChangePonyInput ()
    {
        if (Input.GetAxis("Change") > 0.2f)
        {
            PonyIdx++;
            ChangePony (ponies[Mathf.Abs(PonyIdx)]);
        }

        if (Input.GetAxis("Change") < -0.2f)
        {
            PonyIdx--;
            ChangePony (ponies[Mathf.Abs(PonyIdx)]);
        }
    }

    public void ChangePony (PlayerController newPony)
    {
        foreach (var pony in ponies)
        {
            if (pony == newPony)
            {
                pony.OnDisableAI();
                pony.inputController = Instance;
                pony.inputAI.enabled = false;
                pony.logicAI.enabled = false;
                this.pony = pony;
                transform.SetParent(pony.transform, false);
            }

            else
            {
                pony.OnEnableAI();
                pony.inputController = pony.inputAI;
                pony.inputAI.enabled = true;
                pony.logicAI.enabled = true;
            }
        }
    }
}
