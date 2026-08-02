using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Bootstrap()
    {
        var playerSessionDataHandle = Addressables.LoadAssetAsync<GameData>("GameData");
        await playerSessionDataHandle.Task;

        if(playerSessionDataHandle.Status == 
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            dataLoaded = true;
            gameData = playerSessionDataHandle.Result;
            if (gameData.doorExits == null)
            {
                gameData.doorExits = new SerializedDictionary<string, Vector3>();
            }
            if (string.IsNullOrEmpty(gameData.newDoorGUID))
            {
                Debug.Log("New door GUID is empty! We don't know where to put the player!");
            }
            if (gameData.data.currentTalisman.IsUnityNull())
            {
                gameData.data.currentTalisman = PlayerData.ETalismans.None;
            }
            initTask.SetResult(true);
            return;
        }
        initTask.SetResult(false);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timekeeper = new GameObject("Timekeeper").AddComponent<Timekeeper>();
    }
}