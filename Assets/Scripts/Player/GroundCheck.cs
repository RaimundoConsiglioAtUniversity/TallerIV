using UnityEngine;
using System;

public class GroundCheck : MonoBehaviour
{
    public PlayerController pony;
    public bool IsGrounded => groundCheck.IsTouchingLayers(groundLayer);
    public Collider2D groundCheck;
    public LayerMask groundLayer;

    public event Action<GameObject> OnGroundActions;
    public event Action<GameObject> OnAirActions;

    //--- Methods ---//

    void Awake()
    {
        pony = transform.parent.parent.GetComponent<PlayerController>();
        print($"Pony: {pony.name}");
    }

    void Update() => Check();

    void Check()
    {
        if (IsGrounded)
            OnGroundActions(pony.gameObject);

        else
            OnAirActions(pony.gameObject);
            
    }
}
