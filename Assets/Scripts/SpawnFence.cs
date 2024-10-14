using UnityEngine;
using System.Collections;

public class SpawnFence : MonoBehaviour
{
    // Minimum and maximum time intervals for when the fence will be spawned
    public float minTime = 1f;
    public float maxTime = 5f;

    private Renderer objectRenderer;
    private Collider2D objectCollider;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider2D>(); 
        
        // Starting the coroutine that handles the random appearance of the fence
        StartCoroutine(ToggleAppearance());
    }

    IEnumerator ToggleAppearance()
    {
        while (true)
        {
            float randomTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomTime);
            
            // Toggling the fence's visibility and collider
            bool isVisible = !objectRenderer.enabled;
            objectRenderer.enabled = isVisible;
            objectCollider.enabled = isVisible;
        }
    }
}
