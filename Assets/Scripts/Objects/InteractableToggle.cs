using System;
using UnityEngine;

public class InteractableToggle : InteractiveObject
{
    public static Action<int> ToggleLinkedObjects;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (ToggleTrigger(other))
            InteractedWith();
    }

    protected virtual bool ToggleTrigger(Collider2D trigger)
    {
        PonyType pony = PlayerInput.Instance.pony.tribe;
        return trigger == pony.pony.groundC.groundCheck;
    }

    public override void InteractedWith()
    {
        ToggleLinkedObjects(ID);
    }
}
