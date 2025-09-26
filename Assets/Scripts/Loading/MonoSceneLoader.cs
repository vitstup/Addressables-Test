using UnityEngine;

public class MonoSceneLoader : MonoBehaviour
{
    [SerializeField] private Unloader unloader; // SerializeField, лучше конечно di, но тут и так сойдёт
    [SerializeField] private bool unloadCurrentAfterLoading;
    [SerializeField] private string sceneAddress;

    private SceneLoader sceneLoader = new SceneLoader();

    public async void LoadScene()
    {
        await sceneLoader.Load(sceneAddress);
        if (unloadCurrentAfterLoading)
            unloader.Unload();
    }
}