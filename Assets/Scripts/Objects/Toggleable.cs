using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Toggleable : InteractiveObject
{
    public List<GameObject> onState = new();
    public List<GameObject> offState = new();

    public bool isActive;

    public void OnEnable() => ToggleTrigger.ToggleLinkedObjects += Toggle;
    public void OnDisable() => ToggleTrigger.ToggleLinkedObjects -= Toggle;

    public void MakeToggleFromMap(ToggleData toggle)
    {
        ID = toggle.ID;
        
        toggle.layer.transform.parent = gameObject.transform;

        if (toggle.state)
            onState.Add(toggle.layer);
        else
            offState.Add(toggle.layer);

        if (toggle.usesColour)
        {
            if (toggle.layer.TryGetComponent<Tilemap>(out var tilemap))
                tilemap.color = ToggleData.colours[ID];
                //tilemap.color = toggle.colour;
            else
                Debug.LogWarning($"{toggle.layer.name} is marked to use colour but has no Tilemap component!");
        }
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

    // public override void Awake() => SwitchStates();

    public void SwitchStates()
    {
        foreach (GameObject layer in onState)
            layer.SetActive(isActive);

        foreach (GameObject layer in offState)
            layer.SetActive(!isActive);
    }
}
