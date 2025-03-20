using UnityEngine;

public class ToggleData
{
    public GameObject layer;
    public int ID;
    public bool state;
    public Color colour;
    public bool usesColour;

    public ToggleData(GameObject layer)
    {
        this.layer = layer;
        ID = -1;
        state = false;
        colour = Color.white;
        usesColour = false;
    }
}
