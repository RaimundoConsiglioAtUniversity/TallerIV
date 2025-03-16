using System.Collections;
using UnityEngine;

public class PonyGaea : PonyType
{
    public float groundPoundStrength = 300f;
    public Collider2D groundPoundCol;
    public bool isGroundPounding;
    public GrowPlant plant;

    private void OnTriggerStay2D(Collider2D other)
    {
        // print($"OnTriggerEnter: {other.name}");

        if (other.GetComponentInParent<GrowPlant>() != null)
        {
            print($"Plant Got!");
            plant = other.GetComponent<GrowPlant>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // print($"OnTriggerExit: {other.name}");
        if (other.GetComponentInParent<GrowPlant>() != null)
        {
            print($"Plant Away");
            plant = null;
        }
    }
    
    public override void Action1()
    {
        //
        if (!isGroundPounding)
        {
            if (pony.groundC.IsGrounded)
                StartCoroutine(GroundPound(true));

            else
                StartCoroutine(GroundPound());
        
        }

        if (pony.groundC.IsGrounded)
            isGroundPounding = false;

    }

    private IEnumerator GroundPound(bool preJump = false)
    {
        if (preJump)
            pony.DoJump(pony.stats.jumpStrength * 1.2f);
        
        if (pony.rb.velocity.y > 0f)
        {
            yield return null;
            StartCoroutine(GroundPound());
        }
        else
        {
            pony.rb.AddForce(Vector2.down * groundPoundStrength);
            isGroundPounding = true;
        }
        
    }

    public override void Action2()
    {
        print("Tried Calling Action2");

        if (plant != null)
        {
            print("Plant is not Null");
            plant.Interact();
        }
        else
        {
            print("Plant is Null");
        }
    }

    public override void OnDisableAI() {}

    public override void OnEnableAI() {}
}
