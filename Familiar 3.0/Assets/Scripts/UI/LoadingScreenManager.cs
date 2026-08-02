using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    // We are using auto properties here BTW
    public static LoadingScreenManager Instance {get; private set;}

    private GameObject loadingSceenCanvas;
    private Slider progress; // If we want to add a loading bar on the loading screen

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameObject host = new GameObject("LoadingScreenManager");
        Instance = host.AddComponent<LoadingScreenManager>();
        DontDestroyOnLoad(host); // Prevents the loading screen game object from disappearing when we actaully load the new scene
    }

    private async Task ShowLoadingScreen()
    {
        if(loadingSceenCanvas == null)
        {
            // Same as instatiate, just done async
            var loadingScreenHandle = Addressables.InstantiateAsync("LoadingScreen");
            await loadingScreenHandle.Task;
            loadingSceenCanvas = loadingScreenHandle.Result; // Set our reference to the load result
            DontDestroyOnLoad(loadingSceenCanvas);

            progress = loadingSceenCanvas.GetComponentInChildren<Slider>();
        }

        // Display loading screen canvas
        loadingSceenCanvas.SetActive(true);
    }

    private async Task HideLoadingScreen()
    {
        if(progress)
            progress.value = 1f;
        await Task.Delay(500); // Prevents the loading screen from snapping away
        loadingSceenCanvas.SetActive(false);
        progress.value = 0f;
    }

    private async Task<bool> LoadSceneAsync(string sceneName)
    {
        var locationHandle = Addressables.LoadResourceLocationsAsync(sceneName, typeof(SceneInstance));
        await locationHandle.Task;

        if (locationHandle.Status != AsyncOperationStatus.Succeeded || locationHandle.Result.Count == 0)
        {
            Addressables.Release(locationHandle);
            Debug.LogError($"Addressable scene '{sceneName}' was not found.");
            return false;
        }

        Addressables.Release(locationHandle);

        AsyncOperationHandle<SceneInstance> sceneInstance = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false, 100);

        while (!sceneInstance.IsDone)
        {
            if (progress)
            {
                progress.value = sceneInstance.PercentComplete;
            }

            // Lets loading continue instead of waiting in the while loop
            await Task.Yield();
        }

        if (progress)
            progress.value = 1f;

        if (sceneInstance.Status == AsyncOperationStatus.Succeeded)
        {
            SceneInstance loadedInstance = sceneInstance.Result;
            GameManager.gameData.data.sceneInstance = loadedInstance;
            AsyncOperation activateOp = loadedInstance.ActivateAsync();

            while (!activateOp.isDone)
            {
                await Task.Yield();
            }
            
            return true;
        }
        Debug.Log("Scene Loading Failed");
        return false;
    }

    public async void SwitchScene(string newSceneName)
    {
        // Makes sure we actaully show the loading screen before starting to load the scene
        await ShowLoadingScreen();
        
        Task gameManagerTask = GameManager.InitTask;
        await gameManagerTask;
        Task<bool> sceneTask = LoadSceneAsync(newSceneName);

        // Waits for both the game manager and the scene to load before continueing
        await Task.WhenAll(gameManagerTask, sceneTask);

        if (!sceneTask.Result)
        {
            await HideLoadingScreen();
            return;
        }

        GameManager.Init();
        

        await HideLoadingScreen();
    }
}
