using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForVideos2 : MonoBehaviour
{
    public string scenename = "MainMenu";
    void Start()
    {
        StartCoroutine(VidToMainMenu());
        
    }

    IEnumerator VidToMainMenu()
    {
        yield return new WaitForSeconds(17);
        SceneManager.LoadScene(scenename);
    }

    
}