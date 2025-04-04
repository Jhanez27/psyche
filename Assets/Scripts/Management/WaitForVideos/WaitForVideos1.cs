using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForVideos1 : MonoBehaviour
{
    public string scenename = "RuinsLobbyDCSTScene";
    void Start()
    {
        StartCoroutine(Example());
        
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(15);
        SceneManager.LoadScene(scenename);
    }

    
}