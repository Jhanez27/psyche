using UnityEngine;

public class DestroyTransition : MonoBehaviour
{

    void Awake()
    {
        Destroy(GameObject.FindWithTag("Fade Transition"));
    }

}
