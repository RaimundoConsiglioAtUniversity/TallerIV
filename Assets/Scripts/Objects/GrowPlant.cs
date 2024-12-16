using UnityEngine;

public class GrowPlant : ObjectInteractionTrigger
{
    public CircleCollider2D triggerArea;
    
    public override void Interact()
    {
        print("Entered Plant Interact Method");
        
        foreach (var linkedObject in linkedObjects)
        {
            print($"Entered Foreach Loop, with item {linkedObject.name}");

            linkedObject.InteractedWith();
        }
    }
}
