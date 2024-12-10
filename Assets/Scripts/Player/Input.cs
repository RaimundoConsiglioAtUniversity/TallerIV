using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputAxis
{
    [SerializeField] private float _value;
    public float deadZone;
    public float value
    {
        get => _value;

        set
        {
            if (float.IsFinite(value))
                _value = Mathf.Clamp(value, -1f, 1f);

            else
                _value = 0f;
        }
    }

    public InputAxis (float value, float deadZone = 0f)
    {
        this.value = value;
        this.deadZone = deadZone;
    }

    public int SteppedValue => Mathf.RoundToInt(Mathf.Abs(value));
    public bool Pressed => Mathf.Abs(value) > deadZone;
    public int Sign => (int)Mathf.Sign(value);
}

public abstract class BaseInput : MonoBehaviour
{
    public abstract bool pressedRun { get; }
    public abstract float hInput { get; }
    public abstract float vInput { get; }
    public abstract bool pressedJump { get; }
    public abstract bool pressedAction1 { get; }
    public abstract bool pressedAction2 { get; }
}
public class PlayerInput : BaseInput
{
    public override bool pressedRun => Input.GetButton("Run");
    public override float hInput =>  Input.GetAxis("Horizontal");
    public override float vInput => Input.GetAxis("Vertical");
    public override bool pressedJump => Input.GetButtonDown("Jump");
    public override bool pressedAction1 => Input.GetButtonDown("Action1");
    public override bool pressedAction2 => Input.GetButtonDown("Action2");
}

public class FriendInput : BaseInput
{
    public override bool pressedRun => throw new System.NotImplementedException();

    public override float hInput => throw new System.NotImplementedException();

    public override float vInput => throw new System.NotImplementedException();

    public override bool pressedJump => throw new System.NotImplementedException();

    public override bool pressedAction1 => throw new System.NotImplementedException();

    public override bool pressedAction2 => throw new System.NotImplementedException();
}
