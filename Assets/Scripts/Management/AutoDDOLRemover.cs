using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AutoDDOLRemover : MonoBehaviour
{
    [Header("Choose objects by name or tag to delete")]
    public List<string> namesToDelete = new List<string>();
    public List<string> tagsToDelete = new List<string>();

    private void Awake()
    {
        // Prevent duplicate cleaners
        if (FindObjectsOfType<AutoDDOLRemover>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Ensure this cleaner persists across scenes
        DontDestroyOnLoad(gameObject);

        // Register for scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unregister to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start cleanup coroutine
        StartCoroutine(DeleteRoutine());
    }

    private System.Collections.IEnumerator DeleteRoutine()
    {
        // Wait one frame for scene to initialize
        yield return null;

        // Create a temporary object to identify the current scene
        GameObject temp = new GameObject("TempSceneObject");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(temp, currentScene);

        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        List<GameObject> ddolObjectsToDestroy = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                if (obj == gameObject || obj == temp) continue;

                if (namesToDelete.Contains(obj.name))
                {
                    ddolObjectsToDestroy.Add(obj);
                }
                else
                {
                    try
                    {
                        if (tagsToDelete.Contains(obj.tag))
                        {
                            ddolObjectsToDestroy.Add(obj);
                        }
                    }
                    catch { }
                }
            }
        }

        foreach (GameObject obj in ddolObjectsToDestroy)
        {
            Debug.Log($"[DDOL Cleaner] Destroying: {obj.name}");
            Destroy(obj);
        }

        Destroy(temp);

        // Wait a frame to ensure everything is destroyed cleanly
        yield return null;

        // Now destroy the cleaner itself
        Debug.Log("[DDOL Cleaner] Cleanup complete. Destroying cleaner.");
        Destroy(gameObject);
    }
}
