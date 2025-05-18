using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStoryScript : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    public GameObject turnOnObject;



    

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToWhite();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    public void Play()
    {
        DestroyAllDDOLObjects();
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        turnOnObject.SetActive(true);
        StartCoroutine(LoadSceneRoutine());
    }

    // Call this from a UI Button's OnClick()
    public void DestroyAllDDOLObjects()
    {
        // Create a temporary scene to help detect DDOL objects
        Scene tempScene = SceneManager.CreateScene("TempScene");

        // Create a holder to move into the temp scene (to avoid false positives)
        GameObject tempHolder = new GameObject("TempHolder");
        SceneManager.MoveGameObjectToScene(tempHolder, tempScene);

        // Find all GameObjects in the game (including inactive)
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            // If the object's scene is not the currently active scene and not the temp scene, it's likely DDOL
            if (obj.scene != SceneManager.GetActiveScene() && obj.scene != tempScene)
            {
                Destroy(obj);
            }
        }

        Destroy(tempHolder); // Cleanup
    }

    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
