using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnOffObject : MonoBehaviour
{

    public GameObject gameObjectToTurnOff;

    void OnTriggerEnter2D(Collider2D other)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = (false);
        gameObjectToTurnOff.SetActive(false);    
    }
}
