using Characters;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    //Additional Code
    [SerializeField] private CharactersController playerController;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private bool playTimelineOnEntrance = true;

    private void Awake()
    {
        playerController = Object.FindFirstObjectByType<CharactersController>();
    }
    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            if (playerController != null)
            {
                playerController.transform.position = this.transform.position;
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
            }
            else
            {
                PlayerController.Instance.transform.position = this.transform.position; // Original Code
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
            }

            if (playTimelineOnEntrance)
            {
                timelineManager.PlayOnSceneEnter();
            }
        }
    }
}
