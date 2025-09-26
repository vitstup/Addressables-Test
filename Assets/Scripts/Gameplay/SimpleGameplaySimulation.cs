using UnityEngine;

public class SimpleGameplaySimulation : MonoBehaviour
{
    [SerializeField] private Unloader unloader; // SerializeField, лучше конечно di, но тут и так сойдёт
    [SerializeField] private SimpleLoadAssetData env2Data;
    [SerializeField] private SimpleLoadAssetData veh2Data;

    public async void LoadEnv2()
    {
        foreach (Transform child in env2Data.parent)
        {
            Destroy(child.gameObject);
        }

        unloader.Add(await env2Data.Load());
    }

    public async void LoadVeh2()
    {
        foreach (Transform child in veh2Data.parent)
        {
            Destroy(child.gameObject);
        }

        unloader.Add(await veh2Data.Load());
    }
}