using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;     // Speed of forward/backward movement
    public float rotationSpeed = 200f; // Speed of rotation

    public float playerNumber;
    Rigidbody2D myrb2d;
    private KeyCode lastKey;

    public GameObject collisionPrefab;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public GameObject bananaPrefab;

    public float pauseSecond = 3f;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myrb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.up;
        float rotationInput;
        if (playerNumber == 1){
            rotationInput = -1*Input.GetAxis("Horizontal1");
        }
        else {
            rotationInput = -1*Input.GetAxis("Horizontal2");
        }
        //float rotationInput = -1*Input.GetAxis("Horizontal" + playerNumber); //between -1 and 1
        float angle = rotationInput * rotationSpeed * Time.deltaTime;
        Quaternion rotationQ = Quaternion.AngleAxis(angle, Vector3.forward);
        dir = rotationQ * dir;
        myrb2d.linearVelocity = dir * moveSpeed;
        transform.up = dir;

    }

    // // order left, right, up, down
    // private void faceDirection(KeyCode[] controls)
    // {
    //     Vector2 move = myrb2d.linearVelocity.normalized;
    //     if (Input.GetKey(controls[0]))
    //     {
    //         // animate(side_sprites);
    //         // spriteRenderer.flipX = true;
    //         move.x = -1;
    //         lastKey = controls[0];
    //     }
    //     else if (Input.GetKey(controls[1]))
    //     {
    //         // animate(side_sprites);
    //         // spriteRenderer.flipX = false;
    //         move.x = 1;
    //         lastKey = controls[1];
    //     }
    //     else if (Input.GetKey(controls[2]))
    //     {
    //         // animate(up_sprites);
    //         move.y = 1;
    //         lastKey = controls[2];
    //     }
    //     else if (Input.GetKey(controls[3]))
    //     {
    //         //animate(down_sprites);
    //         move.y = -1;
    //         lastKey = controls[3];
    //     }

    //     float leftRight = Input.GetAxis("Horizontal");
    //     float rotationSpeed = 200f;
    //     transform.Rotate(0, 0, -leftRight * rotationSpeed * Time.deltaTime);


    //     myrb2d.linearVelocity = move * speed;
    // }

    // Handle collision events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a banana
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        else if (collision.gameObject.CompareTag("PeeledBanana"))
        {
            HandlePeeledBananaCollision(collision);
            StartCoroutine(BlinkAndPause());
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

    private void HandlePeeledBananaCollision(Collision2D collision)
    {
        if(playerNumber == 1)
        {
            ScoreManager.player1AddScore(-1);
            Destroy(collision.gameObject);

        }
        else if(playerNumber == 2)
        {
            ScoreManager.player2AddScore(-1);
            Destroy(collision.gameObject);
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

    private IEnumerator BlinkAndPause()
    {
        for(int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.25f);

        }
        spriteRenderer.enabled = true;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseSecond);
        Time.timeScale = 1f; 
    }

}
