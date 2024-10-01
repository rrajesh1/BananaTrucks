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

    enum state {
        driving = 1,
        spinning = 2, 
        collided = 3, 
    }

    private state playerState = state.driving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myrb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerState) 
        {
            case state.spinning:
                Debug.Log("spinning");
                HandleSpin();
                break;
            default:
                HandleMovement();
                break;
        }
        
        
    }

    void HandleSpin()
    {
        float timer = 3f;
        Vector3 speed = new Vector3(0, 100, 0);
        while (timer > 0f) {
            Debug.Log("got here");
            transform.Rotate(speed*Time.deltaTime);
            timer -= Time.deltaTime;
        }
        playerState = state.driving; 
    }

    void HandleMovement()
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

    // Handle collision events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a banana
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        // else if (collision.gameObject.CompareTag("PeeledBanana"))
        // {
        //     HandlePeeledBananaCollision(collision);
        //     //StartCoroutine(BlinkAndPause());
        // }

        // Check if the car collided with another car
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Car collision");
            if (playerState != state.collided)
            {
                HandleCarCollision(collision);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PeeledBanana"))
        {
            HandlePeeledBananaCollision(collision);
            //StartCoroutine(BlinkAndPause());
        }

    }

    // Handle car-to-car collisions
    private void HandleCarCollision(Collision2D collision)
    {
        // float bounceForce = 50f;
        // Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // // Convert positions to Vector2 and calculate the bounce direction
        // Vector2 bounceDirection = myrb2d.position - (Vector2)collision.transform.position;
        // bounceDirection.Normalize();

        // // Apply force to both objects in opposite directions
        // myrb2d.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        // otherRb.AddForce(-bounceDirection * bounceForce, ForceMode2D.Impulse);
        
        playerState = state.collided;
        Vector3 collisionPosition = collision.GetContact(0).point;
        // Instantiate(collisionPrefab, collisionPosition, Quaternion.identity);

        
        // Instantiate(collisionPrefab, collisionPosition, Quaternion.identity);
        StartCoroutine(spawnPeel(collisionPosition, collisionPrefab));
        Debug.Log("Cars collided! Banana peel spawned.");
    }

    private IEnumerator spawnPeel(Vector3 position, GameObject collisionPrefab) 
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(collisionPrefab, position, Quaternion.identity);
        playerState = state.driving;
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

    private void HandlePeeledBananaCollision(Collider2D collision)
    {
        Debug.Log("Peeled banana collision");
        if (playerNumber == 1)
        {
            //ScoreManager.player1AddScore(-1);
            playerState = state.spinning;
            Destroy(collision.gameObject);

        }
        else if(playerNumber == 2)
        {
            //ScoreManager.player2AddScore(-1);
            playerState = state.spinning;
            Destroy(collision.gameObject);
        }
        
    }

    // Spawn a new banana in a random position within the spawn area
    void SpawnNewBanana()
    {
        // Generate a random X and Y position within the defined spawn area
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        // Instantiate the new banana prefab at the random position
        GameObject newBanana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
        newBanana.SetActive(true);
        Debug.Log("New banana spawned at: " + randomPosition);
    }

   IEnumerator BlinkAndPause()
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
