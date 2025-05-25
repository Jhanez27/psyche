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
    [SerializeField]
    private bool makeTransistionConsistent = false; // This is not used in the original code, but added for consistency

    private void Awake()
    {
        playerController = Object.FindFirstObjectByType<CharactersController>();
    }
    private void Start()
    {
        Debug.Log($"Last Load Type at Area Entrance: {SceneManagement.Instance.LastLoadType.ToString()}");
        if (SceneManagement.Instance.LastLoadType == LoadType.LoadGame)
        {
            //Player position declaration is already done in the PlayerController and CharactersController classes
        }
        else if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            Debug.Log("Right Scene");

            //Start happens after Awake, so we can safely assume the playerController and PlayerController.Instance is initialized and replace its position
            if (playerController != null)
            {
                playerController.transform.position = this.transform.position;
            }
            else
            {
                PlayerController.Instance.transform.position = this.transform.position; // Original Code
            }
        }

        UIFade.Instance.FadeToClear();
        CameraController.Instance.SetPlayerCameraFollow();

        Debug.Log($"Timeline is Null: {(timelineManager == null).ToString()}");

        if (timelineManager == null)
        {
            timelineManager = FindObjectOfType<TimelineManager>();
        }

        Debug.Log($"Play Timeline on Entrance: {playTimelineOnEntrance}");

        if (playTimelineOnEntrance && timelineManager != null)
        {
            Debug.Log($"Playing timeline {timelineManager.name}");
            timelineManager.PlayOnSceneEnter();
        }

        SceneManagement.Instance.SetLastLoadType(LoadType.NewGame);
    }
}