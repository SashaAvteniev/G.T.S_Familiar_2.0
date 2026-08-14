using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.ResourceManagement.AsyncOperations;
using RenderSettings = UnityEngine.RenderSettings;

public static class GameManager
{
    public static PlayerMovement player;
    public static GameData gameData;
    public static Timekeeper timekeeper;
    public static bool dataLoaded;

    // Create a task that we can use in other scripts while we await for the data to load
    // Useful for loading screens
    private static TaskCompletionSource<bool> initTask = new TaskCompletionSource<bool>(); 
    public static Task InitTask = initTask.Task;

    // This will create a persistent game manager that allows us to access global static data
    // (e.g. player reference, timekeeper, etc.) globally, removing the requirement to have a
    // game manager in a scene
    
    //This code will run before the scene even loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Bootstrap()
    {
        AsyncOperationHandle<GameData> gameDataHandle = Addressables.LoadAssetAsync<GameData>("GameData");
        await gameDataHandle.Task;

        // Update the session data, and ensure core fields are initialized
        if(gameDataHandle.Status == AsyncOperationStatus.Succeeded)
        {
            dataLoaded = true;
            gameData = gameDataHandle.Result;
            if (gameData.doorExits == null)
            {
                gameData.doorExits = new SerializedDictionary<string, Vector3>();
            }
            if (string.IsNullOrEmpty(gameData.newDoorGUID))
            {
                Debug.Log("New door GUID is empty! We don't know where to put the player!");
            }
            if (gameData.playerData.currentTalisman.IsUnityNull())
            {
                gameData.playerData.currentTalisman = PlayerData.ETalismans.None;
            }
            initTask.SetResult(true);
            return;
        }
        initTask.SetResult(false);
    }
    
    // This initializes core functionality after scene loads
    // Menu UI should be in here eventually...
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timekeeper = new GameObject("Timekeeper").AddComponent<Timekeeper>();
        if (RenderSettings.sun)
        {
            timekeeper.directionalLight = RenderSettings.sun.gameObject;
        }
        // Keeps time constant no matter the scene
        timekeeper.currentTime = gameData.currentTime;
    }
}