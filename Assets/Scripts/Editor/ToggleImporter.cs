using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
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
        
        foreach (var mapLayer in toggleableLayers)
            GetLayerProperties(mapLayer);

        foreach (var toggleGroup in uniqueIDs)
            CreateNewToggleGroup(toggleGroup);

    }

    private void FindToggleableLayers(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        var foundLayers = map.transform.GetChild(0).SearchChildrenByName("Toggle");

        toggleableLayers = toggleableLayers.AddIfUnique(foundLayers);
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
        GameObject toggleObject = new($"ToggleLayer_{toggleGroup}"); // Create the new GameObject
        toggleObject.transform.SetParent(map.transform); // Parent it under the map to embed it in the prefab's hierarchy.
        toggleObject.hideFlags = HideFlags.None; // Clear hide flags to ensure visibility in the Hierarchy.
        var toggleScript = toggleObject.AddComponent<Toggleable>();

        foreach (var toggle in toggles)
        {
            if (toggle.ID != toggleGroup)
                continue;

            toggleScript.MakeToggleFromMap(toggle);
        }
    }
}
