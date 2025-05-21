using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using Unity.Cinemachine;
using Characters;

public class CameraController : Singleton<CameraController>
{
    private CinemachineCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        if (PlayerController.Instance == null)
        {
            cinemachineVirtualCamera.Follow = Object.FindFirstObjectByType<CharactersController>().transform;
            Debug.Log("Characters COntroller Follow");
        }
        else
        {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying CameraController");
    }
}
