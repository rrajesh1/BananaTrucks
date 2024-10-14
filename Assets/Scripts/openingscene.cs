using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    void Update()
    {
        // Checking if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Loading the next scene in the build order
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
