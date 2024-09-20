using UnityEngine;

public class BananaPickup : MonoBehaviour
{
    GameObject Banana;

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject pickupObject = other.gameObject;
        // TODO change this to collect multiple bananas
        if (other.CompareTag("Banana") && Banana == null)
        {
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0.5f, 0, -1f);
            Banana = other.gameObject;
            
        } 

    }


}
