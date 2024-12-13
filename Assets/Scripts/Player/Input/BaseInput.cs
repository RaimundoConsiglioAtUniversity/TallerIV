using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInput : MonoBehaviour
{
    public abstract bool pressedRun { get; }
    public abstract float hInput { get; }
    public abstract float vInput { get; }
    public abstract bool pressedJump { get; }
    public abstract bool heldJump { get; }
    public abstract bool pressedAction1 { get; }
    public abstract bool pressedAction2 { get; }
}
