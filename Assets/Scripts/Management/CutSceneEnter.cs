using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneEnter : MonoBehaviour
{
    public GameObject stopCurrentCanm;
    public GameObject removeEnv;
    //public GameObject thePlayer;
    public GameObject cutsceneCam;
    //public GameObject areaExitOpen;

    
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    

    void OnTriggerEnter2D(Collider2D other)
    {
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
        
        cutsceneCam.SetActive(false);
        //thePlayer.SetActive(true);
        stopCurrentCanm.SetActive(true);
        removeEnv.SetActive(true);
        //areaExitOpen.SetActive(true);

        
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        UIFade.Instance.FadeToBlack();
        StartCoroutine(LoadSceneRoutine());
        
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
