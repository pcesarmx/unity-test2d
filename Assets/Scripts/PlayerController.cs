using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private new Rigidbody2D rigidbody;
    private Vector3 startPosition;
    private float origRunningSpeed;
    private float boostRunningSpeed;
    private const float TOUCHING_DISTANCE = 0.5f;
    private const float BOOST_FACTOR = 0.9f;
    private int healthPoints, manaPoints;

    public float jumpForce = 5f;
    public float runningSpeed = 1.5f;
    public LayerMask groundLayer;
    public Animator animator;
    public static PlayerController sharedInstance;
    public AudioClip sfxKilled;
    public AudioClip sfxSuperjump;
    public AudioClip sfxDamage;

    //MANA
    public const int INITIAL_MANA = 5, MAX_MANA=10, SUPERJUMP_COST = 1;
    public const float SUPERJUMP_FORCE = 1.5f;
    //HEALTH
    public const int INITIAL_HEALTH = 100, MAX_HEALTH = 300, MIN_HEALTH = 10;
    public const float HEALTH_DECREASE_INTERVAL_SECS = 0.5f;

    // Before initialization
    private void Awake()
    {
        sharedInstance = this;
        rigidbody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        origRunningSpeed = runningSpeed;
        boostRunningSpeed = runningSpeed + (runningSpeed * BOOST_FACTOR);
    }

    // Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if ( GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame ) {

            if (Input.GetKey(KeyCode.LeftShift) ) {
                runningSpeed = boostRunningSpeed;
            } else {
                runningSpeed = origRunningSpeed;
            }

            bool s = Input.GetKey(KeyCode.LeftControl);
            if (( Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ) )
            {
                // Debug.Log("SUPERJUMP " + s);
                Jump(s);
            }
            animator.SetBool("isGrounded", IsTouchingTheGround());
        }
    }

    void FixedUpdate()
    {

        if (GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame) {
            //if ((Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D)) && rigidbody.velocity.x < runningSpeed)
            //if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A) && rigidbody.velocity.x > -runningSpeed)

            //Debug.Log("CHECANDO => " + this.rigidbody.position.x + " :imit point => " + LevelGenerator.sharedInstance.getStartXPosOldestLevel());
            if (this.rigidbody.position.x < LevelGenerator.sharedInstance.getStartXPosOldestLevel() && (Input.GetAxisRaw("Horizontal") < 0) )
            {
                // Debug.LogError("Se Paso!!! => " + this.rigidbody.position.x + " :imit point => " + LevelGenerator.sharedInstance.getStartXPosOldestLevel());
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                return;
            }

            if ( Input.GetButton("Horizontal") || Input.GetButtonDown("Horizontal") ) {
                if ( Input.GetAxisRaw("Horizontal") > 0 && rigidbody.velocity.x < runningSpeed)
                {
                    animator.GetComponent<SpriteRenderer>().flipX = false;
                    rigidbody.velocity = new Vector2(runningSpeed, rigidbody.velocity.y);
                    // rigidbody.velocity = new Vector2(runningSpeed * (1 + (this.healthPoints / 100 )) , rigidbody.velocity.y);
                }

                //if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A) && rigidbody.velocity.x > -runningSpeed)
                if (Input.GetAxisRaw("Horizontal") < 0 && rigidbody.velocity.x > -runningSpeed )
                {
                    animator.GetComponent<SpriteRenderer>().flipX = true;
                    // rigidbody.velocity = new Vector2(-(runningSpeed *(1 + (this.healthPoints / 100))), rigidbody.velocity.y);
                    rigidbody.velocity = new Vector2(-runningSpeed , rigidbody.velocity.y);
                }
            } 
        }
    }

    void Jump(bool isSuperJump) {
        if (IsTouchingTheGround() || IsTouchingAWall() ) {
            if (isSuperJump && this.manaPoints >= SUPERJUMP_COST) {
                if (sfxSuperjump != null)
                    GetComponent<AudioSource>().PlayOneShot(sfxSuperjump);
                this.manaPoints -= SUPERJUMP_COST;
                rigidbody.AddForce(Vector2.up * (jumpForce * SUPERJUMP_FORCE), ForceMode2D.Impulse);
            } else {
                // F = m * a ===> a = F / m
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }


        }
    }

    bool IsTouchingTheGround() {
        if(Physics2D.Raycast(this.transform.position,Vector2.down,0.1f,groundLayer ) ) {
            return true;
        }
        return false;
    }


    bool IsTouchingAWall()
    {
        if ( (Physics2D.Raycast(this.transform.position, Vector2.left, TOUCHING_DISTANCE, groundLayer) 
            || Physics2D.Raycast(this.transform.position, Vector2.right, TOUCHING_DISTANCE, groundLayer) )
            && Input.GetButton("Horizontal") ) {
//            Debug.Log(" TOUCHING ");
            return true;
        }
        return false;
    }

    public void kill()
    {
        if (this.animator.GetBool("isAlive") ) {

            StopCoroutine("TirePlayer");
            this.animator.SetBool("isAlive", false);
            if (sfxKilled != null)
            {
                GetComponent<AudioSource>().PlayOneShot(sfxKilled);
            }
            GameManager.sharedInstance.GameOver();
        }
    }

    public void StartGame()
    {
        animator.SetBool("isAlive", true);
        animator.SetBool("isGrounded", true);
        this.transform.position = startPosition;

        this.healthPoints = INITIAL_HEALTH;
        this.manaPoints = INITIAL_MANA;

        StartCoroutine("TirePlayer");
    }

    IEnumerator TirePlayer(){
        while(this.healthPoints>MIN_HEALTH) {
            this.healthPoints--;
            yield return new WaitForSeconds(HEALTH_DECREASE_INTERVAL_SECS);
        }
        yield return null;
    }

    public float GetDistance() {
        return (float) Vector2.Distance(new Vector2(startPosition.x,0), new Vector2(this.transform.position.x,0));
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if ( this.healthPoints >= MAX_HEALTH ) {
            this.healthPoints = MAX_HEALTH;
        }

        //Debug.Log("HEALTH => " + points);
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
        //Debug.Log("MANA => " + points);
    }

    public int GetHealth() {
        return this.healthPoints;
    }

    public int GetMana() {
        return this.manaPoints;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Untagged")
        {
            return;
        }

        // Debug.Log("Bajar energy => " + collision.tag);
        if ( collision.tag == "Enemy" ) {
            this.healthPoints -= 20;
        }

        if (collision.tag == "StaticEnemy")
        {
            this.healthPoints -= 10;
        }

        if (sfxDamage != null && collision.tag.IndexOf("Enemy",0,System.StringComparison.OrdinalIgnoreCase )> -1 )
            GetComponent<AudioSource>().PlayOneShot(sfxDamage);

        if (GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame && this.healthPoints <= 0 ) {
            kill();
        }

    }


}
