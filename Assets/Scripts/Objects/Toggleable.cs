using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Toggleable : InteractiveObject
{
    public List<GameObject> onState;
    public List<GameObject> offState;

    public bool isActive;

    public void OnEnable() => InteractableToggle.ToggleLinkedObjects += Toggle;
    public void OnDisable() => InteractableToggle.ToggleLinkedObjects -= Toggle;


    public void MakeToggleFromMap(int ID, GameObject workingLayer, bool layerState, Color layerColor, bool usesColor)
    {
        this.ID = ID;
        
        workingLayer.transform.parent = gameObject.transform;

        if (layerState == true)
            onState.Add(workingLayer);

        if (layerState == false)
            offState.Add(workingLayer);

        if (usesColor == true)
            workingLayer.GetComponent<Tilemap>().color = layerColor;
    }

    public void Toggle(int ID)
    {
        if (ID == this.ID)
            InteractedWith();
    }

    public override void InteractedWith()
    {
        isActive = !isActive;
        SwitchStates();
    }

    public override void Awake() => SwitchStates();

    public void SwitchStates()
    {
        foreach (GameObject layer in onState)
            layer.SetActive(isActive);

        foreach (GameObject layer in offState)
            layer.SetActive(!isActive);
    }
}
