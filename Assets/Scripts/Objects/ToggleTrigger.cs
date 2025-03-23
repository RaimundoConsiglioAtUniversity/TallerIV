using System;
using UnityEngine;

public class ToggleTrigger : InteractiveObject
{
    public bool startActive;
    public void StartActive(bool active) => startActive = active;

    public SpriteRenderer activityIndicator;

    public static Action<int> ToggleLinkedObjects;


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (Trigger(other))
            InteractedWith();
    }

    protected virtual bool Trigger(Collider2D trigger)
    {
        PonyType pony = PlayerInput.Instance.pony.tribe;
        return trigger == pony.pony.groundC.groundCheck;
    }

    public override void InteractedWith() => ToggleLinkedObjects(ID);
    public void SetColour() => activityIndicator.color = ToggleData.colours[ID];

}
