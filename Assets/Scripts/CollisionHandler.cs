using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    GameSceneManager gameSceneManager;

    private void Start()
    {
        gameSceneManager = FindFirstObjectByType<GameSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameSceneManager.ReloadLevel();
        Debug.Log($"Collision detected with: {other.gameObject.name}");
    }
}
