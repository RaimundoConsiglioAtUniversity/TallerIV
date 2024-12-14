public class FriendInput : BaseInput
{
    public FollowAI friendPony;
    
    void Awake()
    {
        friendPony = GetComponent<FollowAI>();
    }
    
    public override bool pressedRun => !PlayerInput.Instance.StayToggle && friendPony.tryRun;

    public override float hInput => PlayerInput.Instance.StayToggle ? 0f : friendPony.hInput;

    public override float vInput => PlayerInput.Instance.StayToggle ? -1f : friendPony.vInput;

    public override bool pressedJump => !PlayerInput.Instance.StayToggle && friendPony.tryTapJump;

    public override bool heldJump => !PlayerInput.Instance.StayToggle && friendPony.tryHoldJump;

    public override bool pressedAction1 => false;

    public override bool pressedAction2 => false;
}
