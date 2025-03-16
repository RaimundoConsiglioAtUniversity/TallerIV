using UnityEngine;

public class PonyUnicorn : PonyType
{
    public Draggable heldObject = null;
    public Vector3 worldPos;
    public LayerMask tkLayers;


	public bool m_DrawDragLine = true;
	public Color m_Color = Color.cyan;

    public override void Action1()
    {
        //<-< TELEKINESIS >->//

        // print("Attempted Telekinesis");

        if (!heldObject)
            SelectObject();
        
        else
            ReleaseObject();
        
    }

    public override void Action2()
    {
        Teleport();
    }


    private void SelectObject()
    {
        // print("Entered Select Object");
        // Fetch the first collider.
        // NOTE: We could do this for multiple colliders.
        Collider2D collider = Physics2D.OverlapPoint(worldPos, tkLayers);
        if (!collider)
        {    
            // print("No Collider Found!");
            return;
        }

        Draggable newHeldObject = collider.gameObject.GetComponent<Draggable>();

        if (!newHeldObject)
        {    
            // print($"Clicked Object <{collider.name}> Has no Draggable Component!");
            return;
        }
    
        newHeldObject.gameObject.layer = LayerMask.NameToLayer("Holding");
        // Fetch the collider body.
        var rb = collider.attachedRigidbody;
        if (!rb)
        {    
            // print($"Clicked Object <{collider.name}> Has no RigidBody!");
            return;
        }

        // print($"<{collider.name}> Passed all Checks!");
        heldObject = newHeldObject;

        // Add a target joint to the Rigidbody2D GameObject.

        heldObject.joint.enabled = true;

        // Attach the anchor to the local-point where we clicked.
        heldObject.joint.anchor = heldObject.joint.transform.InverseTransformPoint(worldPos);
    }

    private void ReleaseObject()
    {
        if(!heldObject)
            return;
        
        heldObject.gameObject.layer = heldObject.layer;
        heldObject.joint.enabled = false;
        heldObject = null;
    }
    
    void Teleport()
    {
        Vector2 direction = (worldPos - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, worldPos);

        RaycastHit2D[] tpLine = Physics2D.RaycastAll(transform.position, direction, distance);
        bool tpSuccessful = true;

        foreach(var hit in tpLine)
        {
            if (((pony.stats.obstacleLayers.value & (1 << hit.transform.gameObject.layer)) != 0) && !hit.collider.isTrigger && !hit.transform.TryGetComponent(out Permeable _))
            {
                print($"{hit.transform.name} an impassable object, aborting teleport");
                tpSuccessful = false;
            }
        }

        if(tpSuccessful)
        {
            print("Teleported Successfully!");
            pony.rb.position = worldPos;
        }
    }

    void Update()
    {
		// Calculate the world position for the mouse.
		worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        Debug.DrawLine(transform.position, worldPos);

        if (!heldObject)
            return;

		// Update the joint target.
		if (heldObject.joint)
		{
			heldObject.joint.target = worldPos;

			// Draw the line between the target and the joint anchor.
			if (m_DrawDragLine)
				Debug.DrawLine (heldObject.joint.transform.TransformPoint (heldObject.joint.anchor), worldPos, m_Color);
		}
    }

    public override void OnEnableAI() => ReleaseObject();
    public override void OnDisableAI() {}
}
