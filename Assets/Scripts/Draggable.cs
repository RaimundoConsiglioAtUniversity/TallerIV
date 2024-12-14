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
	public float frequency = 5.0f;

	public TargetJoint2D joint;

    void Awake()
    {
        joint = GetComponent<TargetJoint2D>();
		joint.dampingRatio = damping;
		joint.frequency = frequency;
        joint.enabled = false;
    }
}
