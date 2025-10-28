using UnityEngine;

public enum Animations
{
    Idle,
    Walk,
    Trot,
    Gallop,
    Jump_Rise,
    Jump_Peak,
    Fall,
    Flap,
    Duck
}

public class PonyAnim : MonoBehaviour
{
    public Animator controller;
    private Animations currentAnim;

    public void Play(Animations anim)
    {
        
        if (currentAnim != anim || anim == Animations.Flap)
            switch (anim)
            {
                case Animations.Flap:
                    currentAnim = Animations.Flap;
                    controller.ResetTrigger("WingFlap");
                    controller.SetTrigger("WingFlap");
                    Debug.Log("Trigger WingFlap");
                    break;
                case Animations.Jump_Rise:
                    currentAnim = Animations.Jump_Rise;
                    controller.ResetTrigger("Jump");
                    controller.SetTrigger("Jump");
                    break;
                case Animations.Jump_Peak:
                    currentAnim = Animations.Jump_Peak;
                    controller.ResetTrigger("ArcPeak");
                    controller.SetTrigger("ArcPeak");
                    break;
                case Animations.Fall:
                    currentAnim = Animations.Fall;
                    controller.ResetTrigger("Fall");
                    controller.SetTrigger("Fall");
                    break;
                case Animations.Gallop:
                    currentAnim = Animations.Gallop;
                    controller.ResetTrigger("Gallop");
                    controller.SetTrigger("Gallop");
                    break;
                case Animations.Trot:
                    currentAnim = Animations.Trot;
                    controller.ResetTrigger("Trot");
                    controller.SetTrigger("Trot");
                    break;
                case Animations.Walk:
                    currentAnim = Animations.Walk;
                    controller.ResetTrigger("Walk");
                    controller.SetTrigger("Walk");
                    break;
                case Animations.Idle:
                    currentAnim = Animations.Idle;
                    controller.SetTrigger("Idle");
                    break;
                case Animations.Duck:
                    currentAnim = Animations.Duck;
                    controller.SetTrigger("Duck");
                    break;
                default:
                    break;
            }
    }
}
