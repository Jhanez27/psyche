using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    public KeyCode Mapkey;
    public GameObject Map;

    private void Update()
    {
        if (Input.GetKeyDown(Mapkey))
        {
            Map.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    
    public void MouseBack()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
}
