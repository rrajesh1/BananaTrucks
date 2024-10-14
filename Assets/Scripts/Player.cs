using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 10f;
    public float rotationSpeed = 400f;
    public int playerNumber; 
    Rigidbody2D myrb2d;
    public LayerMask mask;

    enum state {
        driving = 0,
        spinning = 1
    }

    bool invulnerable = false;
    private state playerState = state.driving;
    Vector3 movementDiection;

    public GameObject collisionPrefab; 
    public GameObject bananaPrefab;    
    public Vector2 spawnAreaMin;       
    public Vector2 spawnAreaMax;
    public float pauseSecond = 3f;
    public float spinningTimer = 2f;

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

        AudioSource[] audioSources = GetComponents<AudioSource>();
        engineSound = audioSources[0]; 
        collisionSound = audioSources[1]; 
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

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        // Setting of the velocity (only setting the velocity right before the physics resolve collision-- not every frame)
        switch(playerState)
        {
            case state.driving:
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
            if (!engineSound.isPlaying)
            {
                engineSound.Play();
            }
        }
        else
        {
            if (engineSound.isPlaying)
            {
                engineSound.Stop();
            }
        }
    }

    private void HandleSpin()
    {
        myrb2d.linearVelocity = Vector2.zero;
        Vector3 speed = new Vector3(0, 0, 800);
        if (spinningTimer > 0f) {
            Debug.Log("got here");
            transform.Rotate(speed*Time.deltaTime);
            spinningTimer -= Time.deltaTime;

        }
        else 
        {
            playerState = state.driving;
        }
        
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
        if (collision.gameObject.CompareTag("Banana"))
        {
            HandleBananaCollision(collision);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            if (invulnerable) return;
            
            StartCoroutine(playerInvulnerable());
            HandleCarCollision(collision);
        }
    }

    IEnumerator playerInvulnerable()
    {
        invulnerable = true;
        for(int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.25f);

        }
        spriteRenderer.enabled = true;
        invulnerable = false;
    }

    void HandleBananaCollision(Collision2D collision)
    {
        //play banana collision sound
        if (!bananaCollisionSound.isPlaying)
        {
            bananaCollisionSound.Play();
        }

        if (playerNumber == 1)
        {
            ScoreManager.player1AddScore(1);
        }
        else if (playerNumber == 2)
        {
            ScoreManager.player2AddScore(1); 
        }

        Destroy(collision.gameObject);
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
        playerState = state.spinning;
        spinningTimer = 2f;

    }

    private void HandleCarCollision(Collision2D collision)
    {
        Vector3 collisionPosition = collision.GetContact(0).point;

        //Play the collision sound
        if (!collisionSound.isPlaying)
        {
            collisionSound.Play();
        }

        //spawn the bananan peel immediately on collision
        //benno suggested
        SpawnBananaPeel(collisionPosition);
    }

    private void SpawnBananaPeel(Vector3 position)
    {
        GameObject bananaPeel = Instantiate(collisionPrefab, position, Quaternion.identity);
        Collider2D peelCollider = bananaPeel.GetComponent<Collider2D>();
        if (peelCollider != null)
        {
            peelCollider.enabled = false; // Disable the collider
        }
        StartCoroutine(EnableBananaPeelCollider(peelCollider, bananaPeel));
    }

    private IEnumerator EnableBananaPeelCollider(Collider2D peelCollider, GameObject bananaPeel)
    {
        yield return new WaitForSeconds(3f); 
        while (IsOverlappingWithPlayers(bananaPeel))
        {
            yield return null; 
        }

        if (peelCollider != null)
        {
            peelCollider.enabled = true; 
        }
    }

    private bool IsOverlappingWithPlayers(GameObject peel)
    {
        // Checking if the banana peel is overlapping with any player
        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(peel.transform.position, 0.1f, LayerMask.GetMask("Player"));
        return playerColliders.Length > 0;
    }


    // Spawning a new banana at a random position
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

        if (hit1.collider == null && hit2.collider == null)
        {
            GameObject newBanana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
            newBanana.SetActive(true);
        }
        Debug.Log("New banana spawned at: " + randomPosition);
    }
}
