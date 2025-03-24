using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;

    ToggleTrigger[] toggleTriggers = {};
    GameObject[] toggleLayers = {};
    List<ToggleData> toggles = new();
    int[] uniqueIDs = {};

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        GetAllToggleTriggers();
        
        foreach (var toggle in toggleTriggers)
            toggle.SetColour();
        
        GetAllToggleLayers();
        
        foreach (var mapLayer in toggleLayers)
            GetLayerProperties(mapLayer);

        foreach (var toggleGroup in uniqueIDs)
            MakeNewToggleGroup(toggleGroup);

    }

    private void GetAllToggleLayers()
    {
        var foundLayers = map.transform.GetChild(0).SearchChildrenByName("Toggle_");
        toggleLayers = toggleLayers.AddIfUnique(foundLayers);
    }

    private void GetLayerProperties(GameObject mapLayer)
    {
        if (!mapLayer.TryGetComponent(out SuperCustomProperties props))
            return;
        
        ToggleData toggle = new(mapLayer);

        if (props.TryGetCustomProperty("GroupID", out var a))
        {
            toggle.ID = a.GetValueAsInt();
            props.RemoveCustomProperty("GroupID");
        }
        
        uniqueIDs = uniqueIDs.AddIfUnique(toggle.ID);

        if (props.TryGetCustomProperty("IsOnState", out var b))
        {
            toggle.state = b.GetValueAsBool();
            props.RemoveCustomProperty("IsOnState");
        }

        if (props.TryGetCustomProperty("UseTint", out var c))
        {
            toggle.usesColour = c.GetValueAsBool();
            props.RemoveCustomProperty("UseTint");
        }
        if (props.m_Properties.Count == 0)
            Object.DestroyImmediate(props);

        toggles.Add(toggle);
    }

    private void MakeNewToggleGroup(int toggleGroup)
    {
        var toggleObject = new GameObject($"ToggleLayer_{toggleGroup}").ParentToPrefab(map.transform.GetChild(0)); // Create the new GameObject
        var toggleScript = toggleObject.AddComponent<Toggleable>();

        foreach (var toggle in toggles)
        {
            if (toggle.ID != toggleGroup)
                continue;

            toggleScript.MakeToggleFromMap(toggle, toggleTriggers.Where(o => o.startActive && o.ID == toggle.ID).FirstOrDefault());
        }
    }

    private void GetAllToggleTriggers()
    {
        var triggers = map.transform.GetComponentsInChildren<ToggleTrigger>();
        toggleTriggers = toggleTriggers.AddIfUnique(triggers);
    }
}
