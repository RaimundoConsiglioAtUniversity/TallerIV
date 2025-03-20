using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;

    //ToggleData;

    GameObject[] toggleableLayers = {};
    List<int> toggleIDs = new();
    List<bool> toggleStates = new();
    List<Color> toggleColours = new();
    List<bool> toggleUsesCustomColour = new();

    int[] uniqueIDs = {};

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        FindToggleableLayers(args);

        if (map.name != "TestMap (2)")
            return;
        
        foreach (var mapLayer in toggleableLayers)
            GetLayerProperties(mapLayer);

        foreach (var toggleGroup in uniqueIDs)
            CreateNewToggleGroup(toggleGroup);

    }

    private void FindToggleableLayers(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        if (map.name != "TestMap (2)")
            return;
        
        Debug.Log(map.name);
        Debug.Log(map.transform.GetChild(0).name);

        var foundLayers = map.transform.GetChild(0).SearchChildrenByName("Toggle");
        Debug.Log($"Found layers: {foundLayers.Count}");

        toggleableLayers = toggleableLayers.AddIfUnique(foundLayers);
        
        Debug.Log($"{map.name}'s ToggleableLayers (of length {toggleableLayers.Length}) are:");

        string s = "";

        foreach (var layer in toggleableLayers)
            s += $"{layer.name}, ";

        Debug.Log(s);
    }

    private void GetLayerProperties(GameObject mapLayer)
    {
        if (!mapLayer.TryGetComponent(out SuperCustomProperties props))
            return;

        if (props.TryGetCustomProperty("GroupID", out var a))
        {
            //Debug.Log($"{mapLayer.name}'s \"GroupID\" is {a.GetValueAsInt()}");
            toggleIDs.Add(a.GetValueAsInt());
            uniqueIDs = uniqueIDs.AddIfUnique(a.GetValueAsInt());
        }
        else
        {
            //Debug.Log($"Found no \"GroupID\" in {mapLayer.name}, defaulting to '-1'");
            toggleIDs.Add(-1);
        }

        if (props.TryGetCustomProperty("IsOnState", out var b))
        {
            //Debug.Log($"{mapLayer.name}'s \"IsOnState\" is {b.GetValueAsBool()}");
            toggleStates.Add(b.GetValueAsBool());
        }
        else
        {
            //Debug.Log($"Found no \"IsOnState\" in {mapLayer.name}, defaulting to 'false'");
            toggleStates.Add(false);
        }

        if (props.TryGetCustomProperty("Color", out var c))
        {
            //Debug.Log($"{mapLayer.name}'s \"Color\" is {c.GetValueAsColor().ToHexString()}");
            toggleColours.Add(c.GetValueAsColor());
        }
        else
        {
            //Debug.Log($"Found no \"Color\" in {mapLayer.name}, defaulting to '#FFFFFFFF'");
            toggleColours.Add(Color.white);
        }


        if (props.TryGetCustomProperty("UseCustomColor", out var d))
        {
            //Debug.Log($"{mapLayer.name}'s \"UseCustomColor\" is {d.GetValueAsBool()}");
            toggleUsesCustomColour.Add(d.GetValueAsBool());
        }
        else
        {
            //Debug.Log($"Found no \"UseCustomColor\" in {mapLayer.name}, defaulting to 'false'");
            toggleUsesCustomColour.Add(false);
        }


        // toggleIDs.Add(props.TryGetCustomProperty("GroupID", out var a) ? a.GetValueAsInt(): -1);
        // toggleStates.Add(props.TryGetCustomProperty("IsOnState", out var b) && b.GetValueAsBool());
        // toggleColours.Add(props.TryGetCustomProperty("Color", out var c) ? c.GetValueAsColor() : Color.white);
        // toggleUsesCustomColour.Add(props.TryGetCustomProperty("UseCustomColor", out var d) && d.GetValueAsBool());

    }

    private void CreateNewToggleGroup(int toggleGroup)
    {
        GameObject toggleObject = new($"ToggleLayer_{toggleGroup}");
        Debug.Log($"instantiated new ToggleLayer: ToggleLayer_{toggleGroup}, which should have the name {toggleObject.name}");
        var toggleScript = toggleObject.AddComponent<Toggleable>();

        foreach (var layer in toggleableLayers)
        {

            if (!toggleIDs.Contains(toggleGroup))
                continue;

            List<int> groupIDs = toggleIDs.FindAll(i => i == toggleGroup);
            

                
            }

            if (i != toggleGroup)
                continue;

            var workingLayer = toggleableLayers[i];
            var layerState = toggleStates[i];
            var layerColor = toggleColours[i];
            var usesColor = toggleUsesCustomColour[i];

            toggleScript.MakeToggleFromMap(toggleGroup, workingLayer, layerState, layerColor, usesColor);
        }
    }
}

public class ToggleData
{
    GameObject layer;
    int ID;
    bool states;
    Color colour;
    bool usesColour;
}
