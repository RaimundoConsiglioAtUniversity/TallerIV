using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;

    GameObject[] toggleableLayers = {};
    List<ToggleData> toggles = new();

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
        
        ToggleData toggle = new(mapLayer);

        if (props.TryGetCustomProperty("GroupID", out var a))
            toggle.ID = a.GetValueAsInt();
            uniqueIDs = uniqueIDs.AddIfUnique(toggle.ID);

        if (props.TryGetCustomProperty("IsOnState", out var b))
            toggle.state = b.GetValueAsBool();

        if (props.TryGetCustomProperty("Color", out var c))
            toggle.colour = c.GetValueAsColor();

        if (props.TryGetCustomProperty("UseCustomColor", out var d))
            toggle.usesColour = d.GetValueAsBool();

        toggles.Add(toggle);
    }

    private void CreateNewToggleGroup(int toggleGroup)
    {
        // Create the new GameObject
        GameObject toggleObject = new($"ToggleLayer_{toggleGroup}");
        
        // Parent it under the main map to embed it in the prefab hierarchy.
        toggleObject.transform.SetParent(map.transform);
        
        // Optionally, clear hide flags to ensure visibility in the Hierarchy.
        toggleObject.hideFlags = HideFlags.None;

        Debug.Log($"instantiated new ToggleLayer: ToggleLayer_{toggleGroup}, which should have the name {toggleObject.name}");
        var toggleScript = toggleObject.AddComponent<Toggleable>();

        foreach (var toggle in toggles)
        {
            if (toggle.ID != toggleGroup)
                continue;

            toggleScript.MakeToggleFromMap(toggle);
        }
    }
}
