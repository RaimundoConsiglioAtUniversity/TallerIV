using System;
using UnityEngine;

public class InteractableToggle : InteractiveObject
{
    public static Action<int> ToggleLinkedObjects;

    void OnTriggerEnter2D(Collider2D other)
    {
        PonyType pony = PlayerInput.Instance.pony.tribe;
        
        if (other == pony.pony.groundC.groundCheck)
            InteractedWith();
        
    }

    public override void InteractedWith()
    {
        ToggleLinkedObjects(ID);
    }


}
