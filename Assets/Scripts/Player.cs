using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 10;
    public float playerNumber;
    Rigidbody2D myrb2d; 
    private KeyCode lastKey;

    public GameObject collisionPrefab;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public GameObject bananaPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myrb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode[] controls;
        if (playerNumber == 1)
        {
            controls = new [] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        }
        else
        {
            controls = new [] { KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S };
        }
        faceDirection(controls);
    }

     // order left, right, up, down
    private void faceDirection(KeyCode[] controls)
    {
        Vector2 move = myrb2d.linearVelocity.normalized;
        if (Input.GetKey(controls[0]))
        {
            // animate(side_sprites);
            // spriteRenderer.flipX = true;
            move.x = -1;
            lastKey = controls[0];
        }
        else if (Input.GetKey(controls[1]))
        {
            // animate(side_sprites);
            // spriteRenderer.flipX = false;
            move.x = 1;
            lastKey = controls[1];
        }
        else if (Input.GetKey(controls[2]))
        {
            // animate(up_sprites);
            move.y = 1;
            lastKey = controls[2];
        }
        else if (Input.GetKey(controls[3]))
        {
            //animate(down_sprites);
            move.y = -1;
            lastKey = controls[3];
        }

	    float leftRight = Input.GetAxis("Horizontal");
        float rotationSpeed = 200f;
	    transform.Rotate(0, 0, -leftRight * rotationSpeed * Time.deltaTime);

        
        myrb2d.linearVelocity = move * speed;  
    }

    // Handle collision events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a banana
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        // Check if the car collided with another car
        else if (collision.gameObject.CompareTag("Player")) 
        {
            HandleCarCollision(collision);
        }
    }

    // Handle car-to-car collisions
    private void HandleCarCollision(Collision2D collision)
    {
        // Get the contact point of the collision
        ContactPoint2D contact = collision.GetContact(0);
        Vector3 collisionPosition = contact.point;

        // Instantiate the collision prefab at the contact point
        Instantiate(collisionPrefab, collisionPosition, Quaternion.identity);

        Debug.Log("Cars collided! Collision prefab instantiated.");
    }

    // Handle car-to-banana collisions
    private void HandleBananaCollision(Collision2D collision)
    {
        // Add score based on the player number
        if (playerNumber == 1)
        {
            ScoreManager.player1AddScore(1);
            // Destroy the banana that was hit
            Destroy(collision.gameObject);

            // Spawn a new banana after the old one is collected
            SpawnNewBanana();
        }
        else if (playerNumber == 2)
        {
            ScoreManager.player2AddScore(1);

            // Destroy the banana that was hit
            Destroy(collision.gameObject);

            // Spawn a new banana after the old one is collected
            SpawnNewBanana();
        }

    }

    // Spawn a new banana in a random position within the spawn area
    private void SpawnNewBanana()
    {
        // Generate a random X and Y position within the defined spawn area
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        // Instantiate the new banana prefab at the random position
        Instantiate(bananaPrefab, randomPosition, Quaternion.identity);

        Debug.Log("New banana spawned at: " + randomPosition);
    }

}
        