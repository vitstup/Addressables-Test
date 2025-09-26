using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image fill;

    public void Load(IAsyncOperation operation)
    {
        StartCoroutine(UpdateProgress(operation));
    }

    private IEnumerator UpdateProgress(IAsyncOperation operation)
    {
        while (!operation.isDone)
        {
            float progress = operation.progress;
            fill.fillAmount = progress;
            yield return null;
        }
    }
}

public interface IAsyncOperation
{
    public bool isDone { get; }
    public float progress { get; }
}