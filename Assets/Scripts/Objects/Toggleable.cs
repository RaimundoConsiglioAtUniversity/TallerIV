using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggleable : InteractiveObject
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Collider2D col;

    public bool isActive;

    public void OnEnable()
    {
        InteractableToggle.ToggleLinkedObjects += Toggle;
    }

    public void Toggle(int ID)
    {
        if (ID == this.ID)
            InteractedWith();
        
    }
    
    public override void InteractedWith()
    {
        isActive = !isActive;
        
        col.enabled = isActive;
        sprite.enabled = isActive;
    }

    public override void Awake()
    {
        col = GetComponentInChildren<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        col.enabled = isActive;
        sprite.enabled = isActive;
    }
}
