using System.Collections;
using UnityEngine;

public class PonyPegasus : PonyType
{
    public Electric electric;
    public Collider2D thunder;
    public SpriteRenderer thunderSprite;
    
    
    public override void Action1()
    {
        StartCoroutine(ShowThunder());

        if (electric != null)
            electric.Interact();
    }

    public override void Action2()
    {
        //
    }

    public override void OnDisableAI()
    {
    }

    public override void OnEnableAI()
    {
    }

    public IEnumerator ShowThunder()
    {
        thunderSprite.enabled = true;
        
        yield return new WaitForSeconds(1f);

        thunderSprite.enabled = false;
    }
}
