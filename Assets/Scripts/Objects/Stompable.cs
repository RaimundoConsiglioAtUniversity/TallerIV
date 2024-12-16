using UnityEngine;

public class Stompable : InteractiveObject
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PonyGaea gaea = PlayerInput.Instance.pony.tribe is PonyGaea ? (PonyGaea)PlayerInput.Instance.pony.tribe : null;
        
        if (other == gaea.groundPoundCol && gaea.isGroundPounding)
            InteractedWith();
        
    }

    public override void InteractedWith()
    {
        Destroy(gameObject);
    }
}
