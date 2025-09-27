using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image fill;

    public void Load(IAsyncOperation operation)
    {
        _ = UpdateProgress(operation);
    }

    private async UniTask UpdateProgress(IAsyncOperation operation)
    {
        while (!operation.isDone)
        {
            float progress = operation.progress;
            fill.fillAmount = progress;
            await UniTask.WaitForEndOfFrame();
        }
    }
}

public interface IAsyncOperation
{
    public bool isDone { get; }
    public float progress { get; }
}