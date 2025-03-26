using SuperTiled2Unity;
using SuperTiled2Unity.Editor;

[AutoCustomTmxImporter()]
public class ScreenImporter : CustomTmxImporter
{
    private SuperMap map;

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        map = args.ImportedSuperMap;

        // GetAllScreens();
    }

    private void GetAllScreens()
    {
        var importedScreens = map.transform.GetComponentsInChildren<Screen>();
        Screen.levelArea = Screen.levelArea.AddIfUnique(importedScreens,true);
    }
}
