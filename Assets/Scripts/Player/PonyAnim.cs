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
}

public class PonyAnim : MonoBehaviour
{
    public Animator controller;
    private Animations currentAnim;

    public void Play(Animations anim)
    {
        
        if (currentAnim != anim)
            switch (anim)
            {
                case Animations.Flap:
                    currentAnim = Animations.Flap;
                    controller.SetTrigger("Jump_Flap");
                    controller.SetTrigger("Jump_Flap");
                    break;
                case Animations.Jump_Rise:
                    currentAnim = Animations.Jump_Rise;
                    controller.SetTrigger("Jump_Ground");
                    break;
                case Animations.Jump_Peak:
                    currentAnim = Animations.Jump_Peak;
                    controller.SetTrigger("Jump_Peak");
                    break;
                case Animations.Fall:
                    currentAnim = Animations.Fall;
                    controller.SetTrigger("Fall");
                    break;
                case Animations.Gallop:
                    currentAnim = Animations.Gallop;
                    controller.SetTrigger("Gallop");
                    break;
                case Animations.Trot:
                    currentAnim = Animations.Trot;
                    controller.SetTrigger("Trot");
                    break;
                case Animations.Walk:
                    currentAnim = Animations.Walk;
                    controller.SetTrigger("Walk");
                    break;
                case Animations.Idle:
                    currentAnim = Animations.Idle;
                    controller.SetTrigger("Idle");
                    break;
                default:
                    break;
            }
    }
}
