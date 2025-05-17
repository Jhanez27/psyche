using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadCleaner : MonoBehaviour
{
    [Header("Choose objects by name or tag to delete")]
    public List<string> namesToDelete = new List<string>();
    public List<string> tagsToDelete = new List<string>();

    // Public method to be called by a UI button
    public void DeleteSelectedDontDestroyOnLoadObjects()
    {
        StartCoroutine(DeleteRoutine());
    }

    private IEnumerator DeleteRoutine()
    {
        // Wait until the end of the frame to ensure everything is initialized
        yield return new WaitForEndOfFrame();

        // Create a temp object to detect the active scene
        GameObject temp = new GameObject("TempSceneObject");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(temp, currentScene);

        // Get all GameObjects in the scene, including inactive ones
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        List<GameObject> ddolObjectsToDestroy = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Objects in the "DontDestroyOnLoad" scene are not part of the active scene
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                if (obj == gameObject || obj == temp) continue;

                if (namesToDelete.Contains(obj.name))
                {
                    ddolObjectsToDestroy.Add(obj);
                }
                else
                {
                    // Safe tag check
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
            Debug.Log($"Destroying selected DDOL object: {obj.name}");
            Destroy(obj);
        }

        Destroy(temp);
    }
}
