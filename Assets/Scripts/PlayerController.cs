using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed; // player left right walk speed
    public float bulletSpeed = 5;
    public float jumpingForce;
    private static int score;
    private static int sugarcanehealth;
    float allHealth;
    float health = 100f;
    float attackTimer = 10;

    public GameObject healthBar;

    Animator animator;

    GameObject background;
    GameObject middleWall;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public GameObject sugarcanePrefab;

    public Vector3[] spawnPositions;

    //some flags to check when certain animations are playing
    bool isRunning = false;
    bool isWalking = false;
    bool isFiring = false;
    bool isAttacking = false;
    bool isHurt = false;
    bool jump1 = true;
    bool jump2 = false;
    bool onAir = false;
    private bool onFloor = true; // is player on the ground?

    bool canAttack = false;

    public Text attackTimerText;
    public Text healthText;

    //animation states - the values in the animator conditions
    const int idleState = 0;
    const int walkingState = 1;
    const int runningState = 2;
    const int jumpingState = 3;
    const int firingState = 4;
    const int attackState = 5;
    const int hurtState = 6;

    string currentDirection = "left";
    int currentAnimationState = idleState;

    
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        score = Guards.score;
        sugarcanehealth = Sugarcanes.health;

        allHealth = health - sugarcanehealth;

        healthText.text = "Health: " + (allHealth);
        healthBar.gameObject.GetComponent<Image>().fillAmount = allHealth;
        attackTimerText.text = "Attack Ability: " + Mathf.RoundToInt(attackTimer);
        attackTimer -= Time.deltaTime;


        if (allHealth <= 0)
        {
            allHealth = 0;
            Application.LoadLevel("GameOverScene");
        }

        //Check for keyboard input
        if (Input.GetKeyDown(KeyCode.Z))
        {
            changeState(firingState);
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            Invoke("spawnBullet", 0.3f);

        }

        else if (Input.GetKeyDown(KeyCode.X) && canAttack)
        {
            changeState(attackState);
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(2, 0);
        }

        else if (Input.GetKeyDown(KeyCode.Space) && !isFiring && !isAttacking)
        {
            if (jump1 && onFloor) {
                jump2 = true;
                changeState(jumpingState);
                this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                onFloor = false;
                this.gameObject.GetComponent<Rigidbody2D>().velocity.y.Equals(0);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpingForce);
            }

            else if (jump2 && onAir) {
                Debug.Log("Double");
                jump1 = false;
                onAir = false;
                changeState(jumpingState);
                this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                this.gameObject.GetComponent<Rigidbody2D>().velocity.y.Equals(0);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpingForce);
            }
                
        }
        


        else if (Input.GetKey("left") && !isFiring && !isAttacking)
        {
            changeDirection("right");
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            walkSpeed = 5;
            transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);

            if (onFloor)
            {

                changeState(walkingState);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    changeState(runningState);
                    walkSpeed = 7;
                    transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);

                }
            }


        }

        else if (Input.GetKey("right") && !isFiring && !isAttacking)
        {
            changeDirection("left");
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            walkSpeed = 5;
            transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);

            if (onFloor)
            {
                changeState(walkingState);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    changeState(runningState);
                    walkSpeed = 7;
                    transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
                }
            }

        }

        else
        {
            if (onFloor)
            {
                changeState(idleState);
            }
        }


        //check if run animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("run alien"))
            isRunning = true;
        else
            isRunning = false;

        //check if fire animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("fire alien"))
            isFiring = true;
        else
            isFiring = false;

        //check if attack animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack alien"))
            isAttacking = true;
        else
            isAttacking = false;

        //check if walk animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk alien"))
            isWalking = true;
        else
            isWalking = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt alien"))
            isHurt = true;
        else
            isHurt = false;


        if (attackTimer <= 0.5)

        {    canAttack = true;
             attackTimer = 0;
        }
        else
            canAttack = false;

    }

    //--------------------------------------
    // Change the players animation state
    //--------------------------------------
    void changeState(int state)
    {

        if (currentAnimationState == state)
            return;

        switch (state)
        {

            case walkingState:
                animator.SetInteger("state", walkingState);
                break;

            case runningState:
                animator.SetInteger("state", runningState);
                break;

            case jumpingState:
                animator.SetInteger("state", jumpingState);
                break;

            case idleState:
                animator.SetInteger("state", idleState);
                break;

            case firingState:
                animator.SetInteger("state", firingState);
                break;

            case attackState:
                animator.SetInteger("state", attackState);
                break;

            case hurtState:
                animator.SetInteger("state", hurtState);
                break;

        }

        currentAnimationState = state;
    }

    //--------------------------------------
    // Check if player has collided with the floor
    //--------------------------------------
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            jump1 = true;
            onAir = true;
            onFloor = true;
            changeState(idleState);

        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "guard")
        {
            changeState(hurtState);

        }

        if (collision.gameObject.tag == "guard")
        {
            health--;
        
        }
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Cupcake" && isAttacking == true)

        {
            attackTimer = 10;
            GameObject cupcake = GameObject.Find("Cupcake");
            Destroy(cupcake, 0.02f);
            Application.LoadLevel("You won!");

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "RainTrigger")
        {
            spawnRandomSugarcanes();
        }

        if (collision.gameObject.tag == "DeathFloor")
        {
            Application.LoadLevel("GameOverScene");
        }
    }

    
    //--------------------------------------
    // Flip player sprite for left/right walking
    //--------------------------------------
    void changeDirection(string direction)
    {

        if (currentDirection != direction)
        {
            if (direction == "left")
            {
                transform.Rotate(0, -180, 0);
                currentDirection = "left";
            }
            else if (direction == "right")
            {
                transform.Rotate(0, 180, 0);
                currentDirection = "right";
            }
        }

    }
    


    void spawnBullet()
    {

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);

        Destroy(bullet, 0.58f);

    }

    void spawnRandomSugarcanes()
    {
        GameObject sugarcanes = sugarcanePrefab;

        Quaternion spawnRotation = Quaternion.identity;
        for (int i = 0; i < 10; i++)
        {
            Instantiate(sugarcanePrefab, spawnPositions[i], spawnRotation);
        }

    }

}