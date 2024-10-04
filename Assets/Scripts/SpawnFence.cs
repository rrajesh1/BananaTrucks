using UnityEngine;
using System.Collections;

public class SpawnFence : MonoBehaviour
{
    // Minimum and maximum time intervals
    public float minTime = 1f;
    public float maxTime = 5f;

    private Renderer objectRenderer;
    private Collider2D objectCollider;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider2D>(); 
        
        // Start the coroutine to handle random appearance
        StartCoroutine(ToggleAppearance());
    }

    IEnumerator ToggleAppearance()
    {
        while (true)
        {
            // Generate a random time interval
            float randomTime = Random.Range(minTime, maxTime);
            
            // Wait for that amount of time
            yield return new WaitForSeconds(randomTime);
            
            // Toggle the GameObject's visibility and collision
            bool isVisible = !objectRenderer.enabled;
            objectRenderer.enabled = isVisible;
            objectCollider.enabled = isVisible;
        }
    }
}
