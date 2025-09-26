using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = UnityEngine.Debug;

public class AssetLoader : IUnloadeble
{
    private GameObject cached;

    private string assetId;

    public async Task<GameObject> Load(string assetId)
    {
        try
        {
            this.assetId = assetId;
            
            var sizeHandle = Addressables.GetDownloadSizeAsync(assetId);
            await sizeHandle.Task;
            long size = sizeHandle.Status == AsyncOperationStatus.Succeeded ? sizeHandle.Result : -1;
            Addressables.Release(sizeHandle);

            if (size > 0)
                Debug.Log($"Asset {assetId} requires download: {(size / (1024f * 1024f)):F2} MB");

            Stopwatch sw = Stopwatch.StartNew();

            var handle = Addressables.InstantiateAsync(assetId);
            cached = await handle.Task;

            sw.Stop();

            if (handle.Status != AsyncOperationStatus.Succeeded || cached == null)
                throw new NullReferenceException($"Asset {assetId} failed to load");

            Debug.Log($"Asset {assetId} loaded in {sw.ElapsedMilliseconds} ms");

            return cached;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load asset {assetId}. Reason: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }

    public async Task<T> Load<T>(string assetId)
    {
        await Load(assetId);

        if (cached.TryGetComponent(out T comp) == false)
            throw new NullReferenceException($"Asset of type {typeof(T)} is null");

        return comp;
    }

    public async Task Unload()
    {
        UnloadInternal();
    }

    private void UnloadInternal()
    {
        if (cached == null)
            return;

        Addressables.Release(cached);
        Debug.Log($"{assetId} Asset released successfully");
        cached = null;
    }
}