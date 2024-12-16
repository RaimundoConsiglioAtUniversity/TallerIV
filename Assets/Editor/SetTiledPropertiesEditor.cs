using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetTiledProperties))]
public class SetTiledPropertiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetTiledProperties myScript = (SetTiledProperties)target;
        
        if (GUILayout.Button("Initialize Properties"))
        {
            myScript.Initialize();
        }
    }
}
