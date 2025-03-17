using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Linq;

[AutoCustomTmxImporter()]
public class ToggleImporter : CustomTmxImporter
{
    private SuperMap map;
    GameObject[] toggleableLayers;
    int[] toggleIDs;
    bool[] toggleStates;
    Color[] toggleColours;
    bool[] toggleUsesCustomColour;

    int[] uniqueIDs;

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
        toggleableLayers = map.transform.SearchChildrenByName("Toggle");
    }


    private void GetLayerProperties(GameObject mapLayer)
    {
        toggleIDs.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "GroupID").Select(o => o.GetValueAsInt()));
        toggleStates.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "IsOnState").Select(o => o.GetValueAsBool()));
        toggleColours.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "Color").Select(o => o.GetValueAsColor()));
        toggleUsesCustomColour.ToList().AddRange(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "UseCustomColor").Select(o => o.GetValueAsBool()));

        uniqueIDs.AddUniqueItems(toggleIDs.ToArray());
    }

    private void CreateNewToggleGroup(int toggleGroup)
    {
        GameObject toggleObject = new($"ToggleLayer_{toggleGroup}");
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
