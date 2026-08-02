#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door), true)]
public class InitDoor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // Reference the target script
        Door script = (Door)target;
        
        
        GUILayout.Space(5);
        // Draw a clean, dedicated button instead of a text field
        if (GUILayout.Button("Initialize Door", GUILayout.Height(30)))
        {
            script.Init();
        }

        // Force ID validation so it always exists when viewing it
        if(string.IsNullOrEmpty(script.ID))
            script.GenerateId();

        GUILayout.Space(5);

        // Draw a clean, dedicated button instead of a text field
        if (GUILayout.Button("Copy Object GUID", GUILayout.Height(30)))
        {
            GUIUtility.systemCopyBuffer = script.ID;
        }

        GUILayout.Space(5);

        DrawDefaultInspector();
    }
}
#endif
