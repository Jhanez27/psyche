using UnityEngine;

public class MsgBoxManagerJane : MonoBehaviour
{
    public GameObject tooltipUI;

    private void Start()
    {
        if (tooltipUI != null)
            tooltipUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            tooltipUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            tooltipUI.SetActive(false);
        }
    }
}