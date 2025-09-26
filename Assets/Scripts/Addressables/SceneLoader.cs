using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Debug = UnityEngine.Debug;

public class SceneLoader : IUnloadeble
{
    private AssetLoader loadingScreenLoader = new AssetLoader();

    private SceneInstance scene;

    public async Task Load(string sceneAddress)
    {
        try
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(sceneAddress);
            await sizeHandle.Task;
            long size = sizeHandle.Status == AsyncOperationStatus.Succeeded ? sizeHandle.Result : -1;
            Addressables.Release(sizeHandle);

            if (size > 0)
                Debug.Log($"Scene {sceneAddress} requires download: {(size / (1024f * 1024f)):F2} MB");
            Stopwatch sw = Stopwatch.StartNew();

            var loadingScreen = await loadingScreenLoader.Load<LoadingScreen>("LoadingScreen");

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(
                sceneAddress,
                UnityEngine.SceneManagement.LoadSceneMode.Single,
                activateOnLoad: false
            );

            loadingScreen.Load(new SceneLoadingAsyncOperation(handle));

            scene = await handle.Task;

            await Task.Delay(300);

            await handle.Result.ActivateAsync();

            sw.Stop();

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new NullReferenceException($" Scene {sceneAddress} failed to load");

            Debug.Log($"Scene {sceneAddress} loaded in {sw.ElapsedMilliseconds} ms");

        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load Scene {sceneAddress}. Reason: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }

    public async Task Unload()
    {
        await UnloadScene();
    }

    private async Task UnloadScene()
    {
        var sceneName = scene.Scene.name;

        var op = Addressables.UnloadSceneAsync(scene);
        await op.Task;

        Debug.Log($"Scene {sceneName} was unloaded");
    }

    public class SceneLoadingAsyncOperation : IAsyncOperation
    {
        private AsyncOperationHandle<SceneInstance> handle;

        public SceneLoadingAsyncOperation(AsyncOperationHandle<SceneInstance> handle)
        {
            this.handle = handle;
        }

        public bool isDone => handle.IsDone;

        public float progress => handle.PercentComplete;
    }
}