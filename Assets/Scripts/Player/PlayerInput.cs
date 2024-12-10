using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : BaseInput
{
    public PlayerController activePony;
    
    public static PlayerInput Instance
    {
        get
        {
            return instance;
        }
    }

    private static PlayerInput instance;

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(this);
        }
    }

    public override bool pressedRun => Input.GetButton("Run");
    public override float hInput =>  Input.GetAxis("Horizontal");
    public override float vInput => Input.GetAxis("Vertical");
    public override bool pressedJump => Input.GetButtonDown("Jump");
    public override bool heldJump => Input.GetButton("Jump");
    public override bool pressedAction1 => Input.GetButtonDown("Action1");
    public override bool pressedAction2 => Input.GetButtonDown("Action2");
}
