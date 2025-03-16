using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public int ID = -1;
    public void GroupID(int id) => ID = id;
    
    public abstract void InteractedWith();

    public virtual void Awake() {}
}
