using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneEnter : MonoBehaviour
{
    public GameObject stopCurrentCanm;
    public GameObject removeEnv;
    public GameObject turnOnGridTileMap;
    //public GameObject thePlayer;
    public GameObject cutsceneCam;
    //public GameObject areaExitOpen;
    public GameObject changeTransitionCam;


    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        turnOnGridTileMap.SetActive(true);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = (false);
        cutsceneCam.SetActive(true);
        stopCurrentCanm.SetActive(false);
        removeEnv.SetActive(false);
        
        //thePlayer.SetActive(false);
        StartCoroutine(FinishCut());
        
    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds(15);
        DestroyAllDDOLObjects();
        //turnOnGridTileMap.SetActive(false);
        cutsceneCam.SetActive(false);
        //thePlayer.SetActive(true);
        changeTransitionCam.SetActive(true);
        //removeEnv.SetActive(true);
        //areaExitOpen.SetActive(true);


        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        UIFade.Instance.FadeToBlack();
        StartCoroutine(LoadSceneRoutine());
        
    }

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
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneToLoad);
    }


    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(TransitionScene());
        }
    }

    private IEnumerator TransitionScene()
    {
        if (UIFade.Instance != null) // Ensure UIFade exists
        {
            UIFade.Instance.FadeToBlack();
            yield return new WaitForSeconds(1.5f); // Wait for fade animation to finish
        }

        SceneManager.LoadSceneAsync("2_RuinsDCSTCutscene");
    }
    */
}
