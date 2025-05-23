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
    [SerializeField] private bool playTimelineOnEntrance = false;

    private void Awake()
    {
        playerController = Object.FindFirstObjectByType<CharactersController>();
    }
    private void Start()
    {
        Debug.Log("Area Entrancce Start");
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {

            Debug.Log("Right Scene");
            if (playerController != null)
            {

                Debug.Log("CharactersController Found!");
                playerController.transform.position = this.transform.position;

                Debug.Log($"Position at {transform.position.ToString()}");
                CameraController.Instance.SetPlayerCameraFollow();
                Debug.Log("CameraController Found!");
                UIFade.Instance.FadeToClear();

            }
            else
            {
                Debug.Log("PlayerController Found!");
                PlayerController.Instance.transform.position = this.transform.position; // Original Code
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
            }
            if (timelineManager == null) return;
            Debug.Log($"Timeline Enabled: {timelineManager == null} and {playTimelineOnEntrance}");
            if (playTimelineOnEntrance)
            {
                Debug.Log($"Playing timeline {timelineManager.name}");
                timelineManager.PlayOnSceneEnter();
            }
        }
    }
}
