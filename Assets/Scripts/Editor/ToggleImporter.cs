using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Collections.Generic;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;

    ToggleTrigger[] toggleTriggers = {};
    GameObject[] toggleableLayers = {};
    List<ToggleData> toggles = new();
    int[] uniqueIDs = {};

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        FindToggleableLayers();
        FindToggleTriggers();
        
        foreach (var mapLayer in toggleableLayers)
            GetLayerProperties(mapLayer);

        foreach (var toggleGroup in uniqueIDs)
            CreateNewToggleGroup(toggleGroup);

        foreach (var toggle in toggleTriggers)
            toggle.SetColour();
    }

    private void FindToggleableLayers()
    {
        var foundLayers = map.transform.GetChild(0).SearchChildrenByName("Toggle");

        toggleableLayers = toggleableLayers.AddIfUnique(foundLayers);
    }

    private void GetLayerProperties(GameObject mapLayer)
    {
        if (!mapLayer.TryGetComponent(out SuperCustomProperties props))
            return;
        
        ToggleData toggle = new(mapLayer);

        if (props.TryGetCustomProperty("GroupID", out var a))
        {
            toggle.ID = a.GetValueAsInt();
            // props.RemoveCustomProperty("GroupID");
        }
        
        uniqueIDs = uniqueIDs.AddIfUnique(toggle.ID);

        if (props.TryGetCustomProperty("IsOnState", out var b))
        {
            toggle.state = b.GetValueAsBool();
            // props.RemoveCustomProperty("IsOnState");
        }

        if (props.TryGetCustomProperty("UseTint", out var c))
        {
            toggle.usesColour = c.GetValueAsBool();
            // props.RemoveCustomProperty("UseTint");
        }
        else
            toggle.usesColour = false;

        // if (props.m_Properties.Count == 0)
        //     Object.DestroyImmediate(props);

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

    private void FindToggleTriggers()
    {

        var triggers = map.transform.GetComponentsInChildren<ToggleTrigger>();

        toggleTriggers = toggleTriggers.AddIfUnique(triggers);
    }
}
