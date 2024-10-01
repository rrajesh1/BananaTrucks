using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 400f;
    public int playerNumber; // Change to int for easier comparison
    Rigidbody2D myrb2d;

    public GameObject collisionPrefab; // Banana peel prefab to spawn on collision
    public GameObject bananaPrefab;    // Banana prefab to spawn after pickup
    public Vector2 spawnAreaMin;       // Spawn area bounds for banana
    public Vector2 spawnAreaMax;
    public float pauseSecond = 3f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        myrb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Rotation
        float rotationInput = (playerNumber == 1) ? -Input.GetAxis("Horizontal1") : -Input.GetAxis("Horizontal2");
        float angle = rotationInput * rotationSpeed * Time.deltaTime;

        Quaternion rotationQ = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 dir = rotationQ * transform.up;

        myrb2d.linearVelocity = dir * moveSpeed;
        transform.up = dir;
    }

    void FixedUpdate()
    {
        // Setting of the velocity (only setting the velocity right before the physics resolve collision-- not every frame)
        myrb2d.linearVelocity = transform.up * moveSpeed;

    }

    private IEnumerator BlinkAndPause()
    {
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(3f);

        }
        spriteRenderer.enabled = true;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseSecond);
        Time.timeScale = 1f;
    }


    // Collision handling
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player collides with a banana
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        else if (collision.gameObject.CompareTag("PeeledBanana"))
        {
            HandlePeeledBananaCollision(collision);
            //StartCoroutine(BlinkAndPause());
        }

        // Check if player collides with another player
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandleCarCollision(collision);
        }
    }

    // Handle banana pickup, score update, and banana respawn
    void HandleBananaCollision(Collision2D collision)
    {
        // Check which player picked up the banana and update their score
        if (playerNumber == 1)
        {
            ScoreManager.player1AddScore(1);  // Add score to player 1
        }
        else if (playerNumber == 2)
        {
            ScoreManager.player2AddScore(1);  // Add score to player 2
        }

        //Destroy the collected banana
        Destroy(collision.gameObject);

        // Spawn a new banana after the current one is collected
        SpawnNewBanana();
    }

    private void HandlePeeledBananaCollision(Collision2D collision)
    {
        if (playerNumber == 1)
        {
            ScoreManager.player1AddScore(-1);
            Destroy(collision.gameObject);

        }
        else if (playerNumber == 2)
        {
            ScoreManager.player2AddScore(-1);
            Destroy(collision.gameObject);
        }

    }

    // Handle player-to-player collision (spawns banana peel or collision object)
    void HandleCarCollision(Collision2D collision)
    {
        Vector3 collisionPosition = collision.GetContact(0).point;
        Instantiate(collisionPrefab, collisionPosition, Quaternion.identity);
        Debug.Log("Cars collided! Banana peel spawned.");
    }

    // Spawn a new banana at a random position
    public void SpawnNewBanana()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        //Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
        GameObject newBanana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
        newBanana.SetActive(true);  // Ensure the new banana is active
        Debug.Log("New banana spawned at: " + randomPosition);
    }
}
