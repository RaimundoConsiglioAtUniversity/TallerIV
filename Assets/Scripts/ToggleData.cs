using System.Collections.Generic;
using UnityEngine;

public class ToggleData
{
    public GameObject layer;
    public int ID;
    public bool state;
    public Color colour;
    public static Dictionary<int, Color> colours = new()
    {
        {-1, Color.white},
        {0, Color.red},
        {1, Color.blue},
        {2, Color.green},
        {3, Color.yellow}
    };
    public bool usesColour;

    public ToggleData(GameObject layer)
    {
        this.layer = layer;
        ID = -1;
        state = false;
        colour = colours[ID];
        usesColour = false;
    }
}
