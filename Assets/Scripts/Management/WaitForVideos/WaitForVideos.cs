using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForVideo : MonoBehaviour
{
    public string scenename = "MainGateCutscene";

    void Start()
    {
        SceneManagement.Instance.SetLastLoadType(LoadType.NewGame);
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene(scenename);
    }
}