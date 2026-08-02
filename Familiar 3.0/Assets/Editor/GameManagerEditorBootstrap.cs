#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor; 
using UnityEditor.SceneManagement; 
using UnityEngine;
using Object = UnityEngine.Object;

// This script is strictly for getting the game manager working during PIE, no other reason
namespace Editor
{
    public class EditorInputDialog : EditorWindow
    {
        private string description;
        private string inputString;
        private Action<string> onOkAction;

        // This is the static method you call from your MenuItem
        public static void Show(string title, string description, string defaultInput, Action<string> onOk)
        {
            EditorInputDialog window = CreateInstance<EditorInputDialog>();
            window.titleContent = new GUIContent(title);
            window.description = description;
            window.inputString = defaultInput;
            window.onOkAction = onOk;
        
            // ShowModal Utility locks focus to this window until it closes
            window.ShowModalUtility(); 
        }

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
        [MenuItem("Tools/Find Object Name by GUID", false, 2000)]
        private static void FindObject()
        {
            EditorInputDialog.Show(
                    "Input GUID",
                    "",
                    "Town_e0ac79fc-6a43-4f1b-b88c-de0856935d77",
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
        
        static GameManagerEditorBootstrap()
        {
            AssignPlayerSessionData();
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            AssignPlayerSessionData();
        }

        private static void AssignPlayerSessionData()
        {
            try
            {
                var guids = AssetDatabase.FindAssets("t:GameData");

                if (guids == null || guids.Length == 0)
                {
                    GameManager.gameData = null;
                    GameManager.dataLoaded = false;
                    return;
                }

                foreach (var g in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var asset = AssetDatabase.LoadAssetAtPath<GameData>(path);
                    if (asset != null)
                    {
                        GameManager.gameData = asset;
                        if (GameManager.gameData.doorExits == null)
                        {
                            GameManager.gameData.doorExits = new SerializedDictionary<string, Vector3>(); 
                        }
                        GameManager.dataLoaded = true;
                        return;
                    }
                }

                GameManager.gameData = null;
                GameManager.dataLoaded = false;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

#endif