using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


[AutoCustomTmxImporter()]
public class ScreenImporter : CustomTmxImporter
{
    private SuperMap map;
    private Screen[] screens = {};
    private Collider2D[] colliders = {};

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        GetAllScreens();

        foreach (var screen in screens)
        {
            if (screen.TryGetComponent(out Collider2D col))
                colliders = colliders.AddIfUnique(col);
        }

        Screen.levelArea = colliders;
    }

    private void GetAllScreens()
    {
        var importedScreens = map.transform.GetComponentsInChildren<Screen>();
        screens = screens.AddIfUnique(importedScreens);
    }
}
