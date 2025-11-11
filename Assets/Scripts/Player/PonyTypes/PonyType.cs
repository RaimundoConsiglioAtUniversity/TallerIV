using System;
using UnityEngine;

public abstract class PonyType : MonoBehaviour
{
    public PlayerController target;
    public PlayerController pony;
    public abstract void Action1();
    public abstract void Action2();
    public abstract void OnEnableAI();
    public abstract void OnDisableAI();

    void Awake() => pony = transform.GetComponent<PlayerController>();
}
