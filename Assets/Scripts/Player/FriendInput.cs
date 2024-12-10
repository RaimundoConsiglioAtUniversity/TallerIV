using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendInput : BaseInput
{
    public FollowAI friendPony;
    
    void Awake()
    {
        friendPony = GetComponent<FollowAI>();
    }
    
    public override bool pressedRun => friendPony.tryRun;

    public override float hInput => friendPony.hInput;

    public override float vInput => friendPony.vInput;

    public override bool pressedJump => friendPony.tryTapJump;

    public override bool heldJump => friendPony.tryHoldJump;

    public override bool pressedAction1 => false;

    public override bool pressedAction2 => false;
}
