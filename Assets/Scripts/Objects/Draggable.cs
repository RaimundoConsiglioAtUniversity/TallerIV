using UnityEngine;

/// <summary>
/// Drag a Rigidbody2D by selecting one of its colliders by pressing the mouse down.
/// When the collider is selected, add a TargetJoint2D.
/// While the mouse is moving, continually set the target to the mouse position.
/// When the mouse is released, the TargetJoint2D is deleted.`
/// </summary>
public class Draggable : MonoBehaviour
{
	//public LayerMask m_DragLayers;

	[Range (0.0f, 100.0f)]
	public float damping = 1.0f;

	[Range (0.0f, 100.0f)]
	public float frequency = 1.0f;

	public TargetJoint2D joint;

	public LayerMask layer;

    void Awake()
    {
		print($"Initialized {gameObject.name}");

		if (joint == null)
			if(!TryGetComponent(out joint))
				joint =  gameObject.AddComponent<TargetJoint2D>();

        joint = GetComponent<TargetJoint2D>();
		joint.maxForce = 700f;
		joint.dampingRatio = damping;
		joint.frequency = frequency;
        joint.enabled = false;

		layer = gameObject.layer;
    }
}
