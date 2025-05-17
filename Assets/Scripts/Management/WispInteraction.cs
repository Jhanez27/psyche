using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class WispInteraction : MonoBehaviour
{
    public GameObject tooltipUI;

    private void Start()
    {
        if (tooltipUI != null)
            tooltipUI.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && tooltipUI != null)
        {
            tooltipUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && tooltipUI != null)
        {
            tooltipUI.SetActive(false);
        }
    }

}
