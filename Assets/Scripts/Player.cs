using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 10f;
    public float rotationSpeed = 400f;
    public int playerNumber; // Change to int for easier comparison
    Rigidbody2D myrb2d;
    public LayerMask mask;

    enum state {
        driving = 0,
        spinning = 1
    }

    bool collided = false;
    private state playerState = state.driving;
    Vector3 movementDiection;

    public GameObject collisionPrefab; // Banana peel prefab to spawn on collision
    public GameObject bananaPrefab;    // Banana prefab to spawn after pickup
    public Vector2 spawnAreaMin;       // Spawn area bounds for banana
    public Vector2 spawnAreaMax;
    public float pauseSecond = 3f;

    private SpriteRenderer spriteRenderer;
    private AudioSource engineSound;
    private AudioSource collisionSound;
    private AudioSource bananaCollisionSound;
    private AudioSource peeledbananaCollisionSound;

    void Start()
    {
        myrb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementDiection = transform.up;

        //engineSound = GetComponent<AudioSource>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        engineSound = audioSources[0]; // Assume the first one is the engine sound
        collisionSound = audioSources[1]; // The second one is the collision sound
        bananaCollisionSound = audioSources[2];
        peeledbananaCollisionSound = audioSources[3];


    }

    private void Update()
    {
        switch(playerState)
        {
            case state.spinning:
                HandleSpin();
                break;
            case state.driving:
                HandleMovement();
                break;      
        }
    }

    void FixedUpdate()
    {
        // Setting of the velocity (only setting the velocity right before the physics resolve collision-- not every frame)
        switch(playerState)
        {
            // TODO add what the rigidbody change should be for spinning
            default:
                myrb2d.linearVelocity = movementDiection * moveSpeed;
                break;

        }

    }

    private void HandleMovement()
    {
        float rotationInput;
        if (playerNumber == 1){
            rotationInput = -1*Input.GetAxis("Horizontal1");
        }
        else {
            rotationInput = -1*Input.GetAxis("Horizontal2");
        }

        PlayEngineSound();

        float angle = rotationInput * rotationSpeed * Time.deltaTime;
        Quaternion rotationQ = Quaternion.AngleAxis(angle, Vector3.forward);
        movementDiection = rotationQ * movementDiection;
        transform.up = movementDiection;
    }

    private void PlayEngineSound()
    {
        if (Input.GetAxis("Horizontal1") != 0 || Input.GetAxis("Horizontal2") != 0)
        {
            // If the engine sound is not playing, start it
            if (!engineSound.isPlaying)
            {
                engineSound.Play();
            }
        }
        else
        {
            // If no input is detected and the engine sound is still playing, stop it
            if (engineSound.isPlaying)
            {
                engineSound.Stop();
            }
        }
    }

    private void HandleSpin()
    {
        float timer = 10f;
        Vector3 speed = new Vector3(0, 0, 50);
        while (timer > 0f) {
            Debug.Log("got here");
            transform.Rotate(speed*Time.deltaTime);
            timer -= Time.deltaTime;
            //Quaternion deltaRotation = Quaternion.Euler(speed * Time.fixedDeltaTime);
            //myrb2d.MoveRotation(myrb2d.rotation + 50f*Time.deltaTime);

        }
        playerState = state.driving;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PeeledBanana"))
        {
            HandlePeeledBananaCollision(collision);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player collides with a banana
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        // Check if player collides with another player
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (collided != true)
            {
                HandleCarCollision(collision);
            }
        }
    }

    // Handle banana pickup, score update, and banana respawn
    void HandleBananaCollision(Collision2D collision)
    {
        //play banana collision sound
        if (!bananaCollisionSound.isPlaying)
        {
            bananaCollisionSound.Play();
        }

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

    private void HandlePeeledBananaCollision(Collider2D collision)
    {
        //playe peeled banana collision sound
        if (!peeledbananaCollisionSound.isPlaying)
        {
            peeledbananaCollisionSound.Play();
        }

        //check if player collides with peeled banana and update their score
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
    private void HandleCarCollision(Collision2D collision)
    {
        collided = true;
        Vector3 collisionPosition = collision.GetContact(0).point;

        //Play the collision sound
        if (!collisionSound.isPlaying)
        {
            collisionSound.Play();
        }

        //spawn the bananan peel immediately on collision
        //benno suggested
        SpawnBananaPeel(collisionPosition);

        //StartCoroutine(spawnPeel(collisionPosition, collisionPrefab));
        Debug.Log("Cars collided! Banana peel spawned.");
    }

    /*private IEnumerator spawnPeel(Vector3 position, GameObject collisionPrefab) 
    {
        yield return new WaitForSeconds(2f);
        Instantiate(collisionPrefab, position, Quaternion.identity);
        collided = false;
    }
    */

    private void SpawnBananaPeel(Vector3 position)
    {
        // Instantiate the banana peel prefab at the collision position
        GameObject bananaPeel = Instantiate(collisionPrefab, position, Quaternion.identity);

        // Disable the banana peel's collider temporarily
        Collider2D peelCollider = bananaPeel.GetComponent<Collider2D>();
        if (peelCollider != null)
        {
            peelCollider.enabled = false; // Disable the collider
        }

        // Start a coroutine to enable the collider after a short delay
        StartCoroutine(EnableBananaPeelCollider(peelCollider, bananaPeel));
    }

    private IEnumerator EnableBananaPeelCollider(Collider2D peelCollider, GameObject bananaPeel)
    {
        // Wait until the players are not overlapping
        yield return new WaitForSeconds(3f); // Short delay for visual feedback

        // Check if the peel is overlapping with players and wait if it is
        while (IsOverlappingWithPlayers(bananaPeel))
        {
            yield return null; // Wait for the next frame
        }

        // Enable the collider to make the peel collectible
        if (peelCollider != null)
        {
            peelCollider.enabled = true; // Enable the collider
        }

        // Optionally, you can add any spawn animation here
    }

    private bool IsOverlappingWithPlayers(GameObject peel)
    {
        // Check if the banana peel is overlapping with any player
        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(peel.transform.position, 0.1f, LayerMask.GetMask("Player"));
        return playerColliders.Length > 0;
    }


    // Spawn a new banana at a random position
    public void SpawnNewBanana()
    {
        Vector3 horizontalOffset = new Vector3(0, 0.1f, 0);
        Vector3 verticalOffset = new Vector3(0.1f, 0, 0);

        float distance = Vector3.Magnitude(2 * verticalOffset);
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position - verticalOffset, new Vector2(0, 1), 1f, mask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - horizontalOffset, new Vector2(1, 0), 1f, mask);
    
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        //Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
        //GameObject newBanana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);

        if (hit1.collider == null && hit2.collider == null)
        {
            GameObject newBanana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
            newBanana.SetActive(true);
        }
          // Ensure the new banana is active
        Debug.Log("New banana spawned at: " + randomPosition);
    }
}
