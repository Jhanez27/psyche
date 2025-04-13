using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTeleportButton : MonoBehaviour
{
    /*
    public Vector3 teleportLocation;
    public string sceneToLoad;

    public Transform Player;
    */

    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    private float waitToLoadTime = 1f;

  
    /*
    public void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Player = player.transform;
    }

    public void Teleport()
    {
        //teleport player to cordinates
        //Player.gameObject.transform.position = teleportLocation;

        //turn back on controller after button turned it off to prevent interference with position
        Player.GetComponent<CharacterController>().enabled = true;
    }
    */

    public void TeleportScene()
    {
        //save cords and load new scene
        //PlayerPrefs.SetFloat("TeleportX", teleportLocation.x);
        //PlayerPrefs.SetFloat("TeleportY", teleportLocation.y);
        //PlayerPrefs.SetFloat("TeleportZ", teleportLocation.z);

        //SceneManager.LoadScene(sceneToLoad);

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
}