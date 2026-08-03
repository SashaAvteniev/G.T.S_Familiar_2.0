#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor; 
using UnityEditor.SceneManagement; 
using UnityEngine;
using Object = UnityEngine.Object;

// This script is strictly for getting the game manager working during PIE
// and to give us a menu option to find object names based on its GUID

namespace Editor
{
    // Menu option
    public class EditorInputDialog : EditorWindow
    {
        private string description;
        private string inputString;
        private Action<string> onOkAction;
        
        // Create the window itself
        public static void Show(string title, string description, string defaultInput, Action<string> onOk)
        {
            EditorInputDialog window = CreateInstance<EditorInputDialog>();
            window.titleContent = new GUIContent(title);
            window.description = description;
            window.inputString = defaultInput;
            window.onOkAction = onOk;
            window.minSize = new Vector2(400, 100);
            window.maxSize = new Vector2(400, 100);
            window.ShowModalUtility(); 
        }
        
        // Define the layout of the window
        private void OnGUI()
        {
            GUILayout.Label(description, EditorStyles.wordWrappedLabel);
            GUILayout.Space(8);
        
            // Capture the text box input
            inputString = EditorGUILayout.TextField(inputString);
            GUILayout.Space(15);
            
            // Layout buttons horizontally at the bottom
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Cancel"))
                {
                    Close();
                }

                if (GUILayout.Button("OK"))
                {
                    onOkAction?.Invoke(inputString);
                    Close();
                }
            }
        }
    }
    
    [InitializeOnLoad]
    internal static class GameManagerEditorBootstrap
    {
        // Add the methods from above to an actual menu item
        [MenuItem("Tools/Find Object Name by GUID", false, 2000)]
        private static void FindObject()
        {
            // Define the dialog
            EditorInputDialog.Show(
                    "Find Object Name by GUID",
                    "Write the name of an object in the console, based on its GUID",
                    "",
                    onOk: (userInput) =>
                    {
                        if (!string.IsNullOrEmpty(userInput))
                        {
                            foreach (InteractParent gameObject in Object.FindObjectsByType<InteractParent>(FindObjectsSortMode.None))
                            {
                                if (gameObject.ID.Equals(userInput))
                                {
                                    Debug.Log("Game Object Found! Name: " + gameObject.name);
                                    return;
                                }
                            }
                            Debug.Log("Could not find object with that GUID!");
                        }
                    }
                );
        }
        
        // To handle the game manager while we are in the editor
        // Nothing here will happen in an actual build of the game
        static GameManagerEditorBootstrap()
        {
            AssignPlayerSessionData();
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            AssignPlayerSessionData();
        }

        // This is future proofed in the event we need more than one game data file.
        // All new ones should be marked with GameData somewhere in the filename.
        private static void AssignPlayerSessionData()
        {
            try
            {
                string[] guids = AssetDatabase.FindAssets("t:GameData");

                if (guids == null || guids.Length == 0)
                {
                    Debug.LogWarning("Could not find any game data assets!");
                    GameManager.gameData = null;
                    GameManager.dataLoaded = false;
                    return;
                }

                foreach (string g in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(g);
                    GameData asset = AssetDatabase.LoadAssetAtPath<GameData>(path);
                    if (asset != null)
                    {
                        // Things in here will need to be added/removed as the codebase grows
                        GameManager.gameData = asset;
                        if (GameManager.gameData.doorExits == null)
                        {
                            GameManager.gameData.doorExits = new SerializedDictionary<string, Vector3>(); 
                        }
                        GameManager.dataLoaded = true;
                        return;
                    }
                }

                Debug.LogWarning("Could not find any game data assets!");
                GameManager.gameData = null;
                GameManager.dataLoaded = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

#endif