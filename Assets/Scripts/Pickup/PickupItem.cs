using Inventory.Model;
using System;
using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO Item { get; private set; } // The item that this pickup represents
    [field: SerializeField]
    public int Quantity { get; set; } = 1; // The quantity of the item in this pickup

    [SerializeField]
    private AudioSource audiosource; // The audio source for playing sounds
    [SerializeField]
    private float duration = 0.3f; // Duration for the pickup animation

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Item.Image; // Set the sprite of the pickup item
    }
    
    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further interactions
        StartCoroutine(PickupAnimation()); // Start the pickup animation
    }

    private IEnumerator PickupAnimation()
    {
        audiosource.Play(); // Play the pickup sound
        Vector3 startScale = transform.localScale; // Get the initial scale of the pickup item
        Vector3 endScale = Vector3.zero;
        float elapsedTime = 0f; // Initialize elapsed time

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Update elapsed time
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration); // Scale the item down
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject); // Destroy the pickup item
    }
}
