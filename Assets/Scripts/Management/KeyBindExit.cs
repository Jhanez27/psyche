using UnityEngine;

public class KeyBindExit : MonoBehaviour
{
    public KeyCode exitOverlayKey;
    public GameObject overlay;

    private void Update()
    {
        if (Input.GetKeyDown(exitOverlayKey))
        {
            overlay.SetActive(false);
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
