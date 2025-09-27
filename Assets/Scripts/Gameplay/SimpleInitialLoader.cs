using Cysharp.Threading.Tasks;
using UnityEngine;

public class SimpleInitialLoader : MonoBehaviour
{
    [SerializeField] private Unloader unloader; // SerializeField, лучше конечно di, но тут и так сойдёт
    [SerializeField] private SimpleLoadAssetData[] data;

    private void Start()
    {
        Init();
    }

    private async void Init()
    {
        foreach(var e in data)
        {
            unloader.Add(await e.Load());
        }
    }
}

[System.Serializable]
public class SimpleLoadAssetData
{
    [field: SerializeField] public string key { get; private set; }
    [field: SerializeField] public Transform parent { get; private set; }

    public async UniTask<AssetLoader> Load()
    {
        var loader = new AssetLoader();
        var obj = await loader.Load(key);

        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        return loader;
    }
}