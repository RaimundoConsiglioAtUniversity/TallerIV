using UnityEngine;

public class PlayerButton : ObjectInteractionTrigger
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other != PlayerInput.Instance.pony.groundC.groundCheck)
        {
            Interact();
        }
    }

    public override void Interact()
    {
            print("Entered Interact Method");
        
        foreach (var linkedObject in linkedObjects)
            {
                print($"Entered Foreach Loop, with item {linkedObject.name}");

                linkedObject.InteractedWith();
            }
    }

}
