using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Linq;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;
    GameObject[] toggleableLayers = {};
    int[] toggleIDs = {};
    bool[] toggleStates = {};
    Color[] toggleColours = {};
    bool[] toggleUsesCustomColour = {};

    int[] uniqueIDs = {};

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        FindToggleableLayers(args);

        if (map.name != "TestMap (2)")
            return;
        
        foreach (var mapLayer in toggleableLayers)
            GetLayerProperties(mapLayer);
        
        if (uniqueIDs == null)
            Debug.LogError("uniqueIDs is null");

        foreach (var toggleGroup in uniqueIDs)
            {
                Debug.Log(toggleGroup);
                CreateNewToggleGroup(toggleGroup);
            }

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

        // Convert the list to an array and assign to toggleableLayers
        toggleableLayers = foundLayers.ToArray();

        //toggleableLayers = toggleableLayers.AddUniqueItems(foundLayers);
        //toggleableLayers.AddUniqueItems(map.transform.GetChild(0).SearchChildrenByName("Toggle"));
        
        Debug.Log($"{map.name}'s ToggleableLayers (of length {toggleableLayers.Length}) are:");

        foreach (var layer in toggleableLayers)
            Debug.Log($"{layer.name}");

    }

    private void GetLayerProperties(GameObject mapLayer)
    {
        toggleIDs.AddUniqueItems(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "GroupID").Select(o => o.GetValueAsInt()).ToArray());
        toggleStates.AddUniqueItems(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "IsOnState").Select(o => o.GetValueAsBool()).ToArray());
        toggleColours.AddUniqueItems(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "Color").Select(o => o.GetValueAsColor()).ToArray());
        toggleUsesCustomColour.AddUniqueItems(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "UseCustomColor").Select(o => o.GetValueAsBool()).ToArray());

        // toggleIDs.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "GroupID").Select(o => o.GetValueAsInt()));
        // toggleStates.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "IsOnState").Select(o => o.GetValueAsBool()));
        // toggleColours.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "Color").Select(o => o.GetValueAsColor()));
        // toggleUsesCustomColour.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "UseCustomColor").Select(o => o.GetValueAsBool()));

        Debug.Log($"{mapLayer.name} IDs:");
        foreach(var id in toggleIDs)
            Debug.Log(id);

        Debug.Log($"{mapLayer.name} States:");
        foreach(var state in toggleStates)
            Debug.Log(state);

        Debug.Log($"{mapLayer.name} Colours:");
        foreach(var col in toggleColours)
            Debug.Log(col);

        Debug.Log($"{mapLayer.name} UseCols: ");
        foreach(var col in toggleUsesCustomColour)
            Debug.Log(col);

        uniqueIDs.AddUniqueItems(toggleIDs.ToArray());
        
        Debug.Log($"Unique IDs: ");
        foreach(var id in uniqueIDs)
            Debug.Log(id);
    }

    private void CreateNewToggleGroup(int toggleGroup)
    {
        GameObject toggleObject = new($"ToggleLayer_{toggleGroup}");
        Debug.Log($"{toggleObject.name}");
        var toggleScript = toggleObject.AddComponent<Toggleable>();

        for (int i = 0; i < toggleIDs.Length; i++)
        {
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
