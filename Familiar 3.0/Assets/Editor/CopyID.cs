#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractParent), true)]
public class CopyID : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // Reference the target script
        InteractParent script = (InteractParent)target;

        // Force ID validation so it always exists when viewing it
        if(string.IsNullOrEmpty(script.ID))
            script.GenerateId();

        GUILayout.Space(5);

        // Draw a clean, dedicated button instead of a text field
        if (GUILayout.Button("Copy Object GUID", GUILayout.Height(30)))
        {
            script.CopyGuidToClipboard();
        }

        GUILayout.Space(5);

        DrawDefaultInspector();
    }
}
#endif
