using UnityEngine;

public class Electric : ObjectInteractionTrigger
{
    PonyPegasus pegasus;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        pegasus = PlayerInput.Instance.pony.tribe is PonyPegasus ? (PonyPegasus)PlayerInput.Instance.pony.tribe : null;

        if (other == pegasus.thunder)
            pegasus.electric = this;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        pegasus = PlayerInput.Instance.pony.tribe is PonyPegasus ? (PonyPegasus)PlayerInput.Instance.pony.tribe : null;

        if (other == pegasus.thunder)
            pegasus.electric = null;
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
