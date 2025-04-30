using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using Unity.Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying CameraController");
    }
}
