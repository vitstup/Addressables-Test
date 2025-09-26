using UnityEngine;

public class Startup : MonoBehaviour
{
    private void Start()
    {
        _ = new SceneLoader().Load("MenuScene");
    }
}