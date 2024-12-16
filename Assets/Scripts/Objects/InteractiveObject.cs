using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public int ID = -1;
    
    public abstract void InteractedWith();

    public virtual void Awake() {}
}
