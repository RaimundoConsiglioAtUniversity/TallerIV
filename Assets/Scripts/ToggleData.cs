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
       {-1, new Color32(0xFF, 0xFF, 0xFF, 0xFF)}, // White
        {0, new Color32(0xFF, 0x60, 0x60, 0xFF)}, // Red
        {1, new Color32(0x60, 0x60, 0xFF, 0xFF)}, // Blue
        {2, new Color32(0x60, 0xFF, 0x60, 0xFF)}, // Green
        {3, new Color32(0xFF, 0xFF, 0x60, 0xFF)}, // Yellow
        {4, new Color32(0xFF, 0x60, 0xFF, 0xFF)}, // Magenta
        {5, new Color32(0x60, 0xFF, 0xFF, 0xFF)}  // Cyan
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
