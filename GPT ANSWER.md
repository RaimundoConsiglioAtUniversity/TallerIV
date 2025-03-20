Hi Bing! I'm having issues with my code. I'm using `SuperTiled2Unity` to Import a Tiled `.tmx` file, but the custom Importer I've made isn't adding the `toggleableLayers` to their proper array, despite the logic finding them properly `behind SearchChildrenByName`. Here are the scripts that manage importing:


`ToggleImporter.cs`
```cs
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

        //toggleableLayers = toggleableLayers.AddIfUnique(foundLayers);
        //toggleableLayers.AddIfUnique(map.transform.GetChild(0).SearchChildrenByName("Toggle"));
        
        Debug.Log($"{map.name}'s ToggleableLayers (of length {toggleableLayers.Length}) are:");

        foreach (var layer in toggleableLayers)
            Debug.Log($"{layer.name}");

    }

    private void GetLayerProperties(GameObject mapLayer)
    {
        toggleIDs.AddIfUnique(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "GroupID").Select(o => o.GetValueAsInt()).ToArray());
        toggleStates.AddIfUnique(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "IsOnState").Select(o => o.GetValueAsBool()).ToArray());
        toggleColours.AddIfUnique(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "Color").Select(o => o.GetValueAsColor()).ToArray());
        toggleUsesCustomColour.AddIfUnique(mapLayer.GetComponents<CustomProperty>().Where(o => o.m_Name == "UseCustomColor").Select(o => o.GetValueAsBool()).ToArray());

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

        uniqueIDs.AddIfUnique(toggleIDs.ToArray());
        
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

```

`Extensions.cs`
```cs
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static void ClampVelocity(this Rigidbody2D rb, float clampX, float clampY)
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -clampX, clampX), Mathf.Clamp(rb.velocity.y, -clampY, clampY));
    }
    public static void AccelerateTo(this Rigidbody2D rb, Vector2 velocity, float accelerationTime, float lerpVal, out float newLerpVal)
    {
        rb.velocity = Vector2.Lerp(rb.velocity, velocity, lerpVal);
        lerpVal += Time.deltaTime / accelerationTime;
        Mathf.Clamp01(lerpVal);
        
        newLerpVal = lerpVal;
    }

    public static Vector3 ArrayProduct(this Vector3 a, Vector3 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;
        float depth = a.z * b.z;

        Vector3 product = new(width, height, depth);

        return product;
    }

    public static Vector3 ArrayProduct(this Vector3 a, Vector2 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;
        float depth = a.z;

        Vector3 product = new(width, height, depth);

        return product;
    }

    public static Vector2 ArrayProduct(this Vector2 a, Vector2 b)
    {
        float width = a.x * b.x;
        float height = a.y * b.y;

        Vector2 product = new(width, height);

        return product;
    }

    public static void ResetOnChange(this float f, float a, float b, float def = 0f)
    {
        if(a != b)
        {
            f = def;
            a = b;
        }
    }

    public static IEnumerable<T> AddIfUnique<T>(this IEnumerable<T> originalArray, IEnumerable<T> newItems)
    {
        if (originalArray == null)
            originalArray = new List<T>();  // Initialize if null

        if (newItems == null)
            newItems = new List<T>();  // Initialize if null

        // Convert array to list for flexibility
        List<T> list = new List<T>(originalArray);

        // Loop through new items and add them if they're not already in the list
        foreach (T item in newItems)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        // Convert the list back to an array and return
        return list.ToArray();
    }

    public static IEnumerable<T> AddIfUnique<T>(this IEnumerable<T> originalArray, T newItem)
    {
        if (originalArray == null)
            originalArray = new T[0];  // Initialize if null

        // Convert array to list for flexibility
        List<T> list = new List<T>(originalArray);

        if (!list.Contains(newItem))
            list.Add(newItem);

        // Convert the list back to an array and return
        return list.ToArray();
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        
        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }

    public static T EnsureComponent<T>(this GameObject gameObject, ref T component) where T : Component
    {
        if (component == null)
            component = gameObject.AddComponent<T>();
        
        return component;
    }

  // marker
public static List<GameObject> SearchChildrenByName(this Transform parent, string searchString)
    {
    List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.gameObject.name.Contains(searchString))
            {
                children.AddIfUnique(child.gameObject);
                Debug.Log($"Found {child.gameObject.name} as a child of {parent.gameObject.name}");
            }

            // Recursively search in the child's children
        children.AddRange(child.SearchChildrenByName(searchString));
        }

    if (children.Count > 0)
        {
            Debug.Log($"{parent.gameObject.name}'s children:");

            foreach (var child in children)
                Debug.Log(child.gameObject.name);
        }

        return children;
    }
}
```
Please wait until I send in the logs as well before responding.

Here are the logs that Unity game me:
Unity Logs:
```cs
TestMap (2)
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:46)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Grid
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:47)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Blocking_Toggle_1 as a child of Blocking
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:129)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:133)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_Off as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:129)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_On_Colour as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:129)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_On_Neutral as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:129)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_AiAvoid as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (at Extensions.cs:129)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found layers: 0
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:50)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

TestMap (2)'s ToggleableLayers (of length 0) are:
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:58)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (at ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters ()
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting ()
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset ()
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets ()
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)
```

What do you think is the problem?

## "GPT RESPONSE"
It looks like you're having an issue where `toggleableLayers` is being populated correctly during the `FindToggleableLayers` method, but for some reason, the expected layers are not showing up as intended (with a length of 0 in the final log). Let’s go over a few things that might be causing this issue:

### 1. The Timing of When Layers Are Found
Your debug logs indicate that the layers are found (`Found Blocking_Toggle_1 as a child of Blocking`, etc.), but then when you log out the `toggleableLayers`, it shows a length of 0. This could be a timing issue related to when the `FindToggleableLayers` method is executed. Specifically:

- **Asset Import Order**: Ensure that the layers are being properly loaded and are available when `FindToggleableLayers` runs. There might be some timing issue where the map or its child objects are not fully initialized or ready by the time you access them.
  
### 2. Search Query Might Not Be Accurate
In the `FindToggleableLayers` method, you are using:

```csharp
var foundLayers = map.transform.GetChild(0).SearchChildrenByName("Toggle");
```

- Make sure the `SearchChildrenByName` method is correctly identifying the layers you want. Even though the logs show that the method finds layers, it's still possible that the query is too strict or doesn’t match the expected child names properly (e.g., a missing or incorrect substring in the search query).

### 3. Investigating Layer Hierarchy
Check the exact hierarchy of objects within `map.transform.GetChild(0)` and verify that the expected child objects are properly named or match the query string "Toggle".

- If the objects in the hierarchy don’t exactly match the search query (e.g., extra spaces or case sensitivity issues), they won’t be included in the search result. 

You can add some more detailed debug information to the `SearchChildrenByName` function to see which objects it finds or doesn’t find.

### 4. Modifying the Extension Method
The `SearchChildrenByName` method in your `Extensions.cs` could also be playing a part here. Specifically, in your recursive search, you're calling `AddIfUnique`, which could be returning an empty array if no matching elements are found. Ensure that it’s correctly adding items to the list, especially in recursive calls.

Additionally, the `AddIfUnique` method creates a new array each time it’s called. Make sure this isn't overwriting previous data in any unintended way. This can be resolved by modifying the array handling to use `List<GameObject>` directly instead of using arrays which get reinitialized in each call.

### Potential Fixes
- **Check for Exact Match**: Ensure the name comparison logic in `SearchChildrenByName` isn’t missing anything due to case sensitivity or other name mismatches. Consider printing out every child name in the hierarchy to debug.
- **Verify Layer Initialization**: Ensure all the map layers are initialized and ready before accessing them. You might want to add more logs and check for `null` references or timing issues.
- **Direct Debugging on Layer Names**: Modify `SearchChildrenByName` to debug each child’s exact name:

  ```csharp
  foreach (Transform child in parent)
  {
      Debug.Log($"Checking child: {child.gameObject.name}");
      if (child.gameObject.name.Contains(searchString))
      {
          children.AddIfUnique(child.gameObject);
      }
  }
  ```

- **Ensure Layers Are Active**: Verify that the layers you're looking for are not disabled or inactive in the scene during the import process.

### Final Check:
After these modifications, you should get a better understanding of whether your layers are being found properly, and if not, it will allow you to trace back why `toggleableLayers` is empty at the end.

Let me know what you find after trying these changes!





Hmm... When adding some debug lines it seems that `AddIfUnique` in conjuntion with `SearchChildrenByName` might be the culprits...

Here are the methods, the methods when combined, and the logs:

`AddIfUnique`:
```cs
    public static IEnumerable<T> AddIfUnique<T>(this IEnumerable<T> originalArray, T newItem)
    {
        if (originalArray == null)
            originalArray = new T[0];  // Initialize if null

        // Convert array to list for flexibility
        List<T> list = new List<T>(originalArray);

        if (!list.Contains(newItem))
        {
            list.Add(newItem);
            Debug.Log($"Added {newItem} to List<{typeof(T).Name}> list");
        }

        Debug.Log($"{typeof(T).Name}s in List<{typeof(T).Name}> list");
        foreach (var i in list)
            Debug.Log($"Found in the List: {i}");

        // Convert the list back to an array and return
        Debug.Log($"{typeof(T).Name}s in List<{typeof(T).Name}> list when converted to an array using list.ToArray()");
        foreach (var i in list.ToArray())
            Debug.Log($"Found in the Array: {i}");
        return list.ToArray();
    }
```

`SearchChildrenByName`
```cs
    public static List<GameObject> SearchChildrenByName(this Transform parent, string searchString)
    {
    List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.gameObject.name.Contains(searchString))
            {
                children.AddIfUnique(child.gameObject);
                Debug.Log($"Found {child.gameObject.name} as a child of {parent.gameObject.name}");
            }

            // Recursively search in the child's children
        children.AddRange(child.SearchChildrenByName(searchString));
        }

    if (children.Count > 0)
        {
            Debug.Log($"{parent.gameObject.name}'s children:");

            foreach (var child in children)
                Debug.Log(child.gameObject.name);
        }

        return children;
    }
```

Combined:
```cs
    public static List<GameObject> SearchChildrenByName(this Transform parent, string searchString)
    {
    List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.gameObject.name.Contains(searchString))
            {
                if (children == null)
                    children = new GameObject[0];  // Initialize if null

                // Convert array to list for flexibility
                List<GameObject> list = new List<GameObject>(children);

                if (!list.Contains(child.gameObject))
                {
                    list.Add(child.gameObject);
                    Debug.Log($"Added {child.gameObject} to List<{typeof(GameObject).Name}> list");
                }

                Debug.Log($"{typeof(GameObject).Name}s in List<{typeof(GameObject).Name}> list");
                foreach (var i in list)
                    Debug.Log($"Found in the List: {i}");

                // Convert the list back to an array and return
                Debug.Log($"{typeof(GameObject).Name}s in List<{typeof(GameObject).Name}> list when converted to an array using list.ToArray()");
                foreach (var i in list.ToArray())
                    Debug.Log($"Found in the Array: {i}");
                // return list.ToArray();

                Debug.Log($"Found {child.gameObject.name} as a child of {parent.gameObject.name}");
            }

            // Recursively search in the child's children
        children.AddRange(child.SearchChildrenByName(searchString));
        }

    if (children.Count > 0)
        {
            Debug.Log($"{parent.gameObject.name}'s children:");

            foreach (var child in children)
                Debug.Log(child.gameObject.name);
        }

        return children;
    }
```

Logs:
```cs
TestMap (2)
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:46)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Grid
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:47)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Added Blocking_Toggle_1 (UnityEngine.GameObject) to List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:109)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:112)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the List: Blocking_Toggle_1 (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:115)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list when converted to an array using list.ToArray()
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:119)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the Array: Blocking_Toggle_1 (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:122)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Blocking_Toggle_1 as a child of Blocking
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:155)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:159)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Added Toggle_1_Off (UnityEngine.GameObject) to List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:109)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:112)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the List: Toggle_1_Off (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:115)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list when converted to an array using list.ToArray()
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:119)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the Array: Toggle_1_Off (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:122)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_Off as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:155)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Added Toggle_1_On_Colour (UnityEngine.GameObject) to List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:109)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:112)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the List: Toggle_1_On_Colour (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:115)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list when converted to an array using list.ToArray()
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:119)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the Array: Toggle_1_On_Colour (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:122)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_On_Colour as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:155)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Added Toggle_1_On_Neutral (UnityEngine.GameObject) to List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:109)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:112)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the List: Toggle_1_On_Neutral (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:115)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list when converted to an array using list.ToArray()
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:119)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the Array: Toggle_1_On_Neutral (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:122)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_On_Neutral as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:155)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Added Toggle_1_AiAvoid (UnityEngine.GameObject) to List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:109)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:112)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the List: Toggle_1_AiAvoid (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:115)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GameObjects in List<GameObject> list when converted to an array using list.ToArray()
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:119)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found in the Array: Toggle_1_AiAvoid (UnityEngine.GameObject)
UnityEngine.Debug:Log (object)
Extensions:AddIfUnique<UnityEngine.GameObject> (System.Collections.Generic.IEnumerable`1<UnityEngine.GameObject>,UnityEngine.GameObject) (@ Extensions.cs:122)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:154)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found Toggle_1_AiAvoid as a child of Grid
UnityEngine.Debug:Log (object)
Extensions:SearchChildrenByName (UnityEngine.Transform,string) (@ Extensions.cs:155)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:49)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

Found layers: 0
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:50)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

TestMap (2)'s ToggleableLayers (of length 0) are:
UnityEngine.Debug:Log (object)
ToggleImporter:FindToggleableLayers (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:58)
ToggleImporter:TmxAssetImported (SuperTiled2Unity.Editor.TmxAssetImportedArgs) (@ ToggleImporter.cs:20)
SuperTiled2Unity.Editor.TmxAssetImporter:RunCustomImporterType (System.Type) (@ Line #523)
SuperTiled2Unity.Editor.TmxAssetImporter:ApplyAutoImporters () (@ Line #499)
SuperTiled2Unity.Editor.TmxAssetImporter:DoCustomImporting () (@ Line #476)
SuperTiled2Unity.Editor.TmxAssetImporter:InternalOnImportAsset () (@ Line #80)
SuperTiled2Unity.Editor.SuperImporter:OnImportAsset (UnityEditor.AssetImporters.AssetImportContext) (@ SuperImporter.cs:41)
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext)
SuperTiled2Unity.Editor.ST2USettingsProvider:ReimportTiledAssets (bool) (@ Line #354)
SuperTiled2Unity.Editor.ST2USettingsProvider:DoGuiReimportAssets () (@ Line #281)
SuperTiled2Unity.Editor.ST2USettingsProvider:OnGUI (string) (@ Line #79)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)
```
