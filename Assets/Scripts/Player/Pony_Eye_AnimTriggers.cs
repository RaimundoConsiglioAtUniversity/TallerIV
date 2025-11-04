using UnityEngine;

public class Pony_Eye_AnimTriggers : MonoBehaviour
{
    public Animator controller;

    public void Eye_Side()
    {
        controller.SetTrigger("Side");
        ChangeAnimationAndKeepFrame("Side");
    }

    public void Eye_Front()
    {
        controller.SetTrigger("Front");
        ChangeAnimationAndKeepFrame("Front");

    }

    public void Eye_3_4()
    {
        controller.SetTrigger("3/4");

        ChangeAnimationAndKeepFrame("3/4");
    }

    public void Eye_SlightDown()
    {
        controller.SetTrigger("SlightDown");
        ChangeAnimationAndKeepFrame("SlightDown");
    }

    public void Eye_MidDown()
    {
        controller.SetTrigger("MidDown");
        ChangeAnimationAndKeepFrame("MidDown");
    }

    public void Eye_Down()
    {
        controller.SetTrigger("Down");
        ChangeAnimationAndKeepFrame("Down");
    }

    public void Eye_Up()
    {
        controller.SetTrigger("Up");
        ChangeAnimationAndKeepFrame("Up");
    }

    public void ChangeAnimationAndKeepFrame(string newAnimationStateName)
    {
        // Get normalized time of the current animation
        AnimatorStateInfo currentStateInfo = controller.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentStateInfo.normalizedTime;

        // Play the new animation state at the same normalized time
        controller.Play(newAnimationStateName, 0, normalizedTime);
    }
}
