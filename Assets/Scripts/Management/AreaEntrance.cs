using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    //Additional Code
    [SerializeField] private CharactersController playerController;

    private void Start()
    {
        Debug.Log(transitionName + " is the transition name, and " + SceneManagement.Instance.SceneTransitionName + " is the sceneManagement.");
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            Debug.Log(playerController != null);
            if (playerController != null)
            {
                playerController.transform.position = this.transform.position;
                UIFade.Instance.FadeToClear();
            }
            else
            {
                PlayerController.Instance.transform.position = this.transform.position; // Original Code
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
            }
        }
    }
}
