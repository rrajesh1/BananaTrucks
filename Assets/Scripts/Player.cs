using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 10;
    public float playerNumber;
    Rigidbody2D myrb2d; 
    private KeyCode lastKey;
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

    private void OnCollisionEnter2D(Collision2D collision){
        if(playerNumber == 1 && collision.gameObject.CompareTag("Banana")){
            ScoreManager.player1AddScore(1);
        }

        else if(playerNumber == 2 && collision.gameObject.CompareTag("Banana")){
            ScoreManager.player2AddScore(1);
        }
    }

}
        