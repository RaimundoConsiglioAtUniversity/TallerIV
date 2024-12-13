using UnityEngine;

public abstract class PonyType : MonoBehaviour
{
    public PlayerController target;
    public PlayerController pony;
    public abstract void Action1();
    public abstract void Action2();

    void Awake() => pony = transform.parent.GetComponent<PlayerController>();
}
