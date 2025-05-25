using Characters;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    private static int totalEntrances;
    private static int completedEntrances;
    private static bool sceneProcessed;


    [SerializeField] private string transitionName;
    //Additional Code
    [SerializeField] private CharactersController playerController;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private bool playTimelineOnEntrance = false;
    [SerializeField]
    private bool makeTransistionConsistent = false; // This is not used in the original code, but added for consistency

    [SerializeField] private AreaEntrance areaEntrance; // This is not used in the original code, but added for consistency

    private void Awake()
    {
        playerController = Object.FindFirstObjectByType<CharactersController>();
    }
    private void Start()
    {
        if (!sceneProcessed)
        {
            totalEntrances = FindObjectsOfType<AreaEntrance>().Length;
            completedEntrances = 0;
            sceneProcessed = true;

            DataPersistenceManager.Instance.LoadGame();
        }

        // Your usual entrance logic here
        HandleEntrance();

        completedEntrances++;

        // Run this ONCE when all AreaEntrances have finished
        if (completedEntrances >= totalEntrances)
        {
            Debug.Log("All AreaEntrances finished. Setting LastLoadType to NewGame.");
            SceneManagement.Instance.SetLastLoadType(LoadType.NewGame);
        }
    }

    private void HandleEntrance()
    {
        Debug.Log($"Area Entrance LastLoadType: {SceneManagement.Instance.LastLoadType} in {SceneManager.GetActiveScene().name}");

        Debug.Log($"Transform Position in {SceneManager.GetActiveScene().name} is {(playerController != null ? playerController.transform.position.ToString() : PlayerController.Instance.transform.position.ToString())} by {transform.name}");

        if (areaEntrance != null)
        {
            Debug.Log($"Area Entrance: {areaEntrance.name} in {SceneManager.GetActiveScene().name}");
            if (playerController != null)
                playerController.transform.position = areaEntrance.transform.position;
            else
            {
                PlayerController.Instance.transform.position = areaEntrance.transform.position;
            }

            SceneManagement.Instance.SetTransitionName("");
            areaEntrance = null;
        }
        else
        {
            if (SceneManagement.Instance.LastLoadType == LoadType.LoadGame)
            {
                // Skip positioning
            }
            else if (transitionName == SceneManagement.Instance.SceneTransitionName)
            {
                if (playerController != null)
                    playerController.transform.position = transform.position;
                else
                {
                    PlayerController.Instance.transform.position = transform.position;
                }

                SceneManagement.Instance.SetTransitionName("");
            }
        }

        UIFade.Instance.FadeToClear();
        CameraController.Instance.SetPlayerCameraFollow();

        if (timelineManager == null)
            timelineManager = FindObjectOfType<TimelineManager>();

        if (playTimelineOnEntrance && timelineManager != null)
            timelineManager.PlayOnSceneEnter();
    }

    private void OnDestroy()
    {
        completedEntrances = 0;
        sceneProcessed = false;
    }
}