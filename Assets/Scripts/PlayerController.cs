using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float velocity; // X speed player
    public float bulletMaxInitialVelocity; // Bullet initial velocity
    public float maxTimeShooting; // Maximum shooting time
    public BoxCollider2D groundBC; // Ref is BoxCollider2D chao
    public GameObject bulletPrefab; // Ref is GameObject (Pre-made) our bullet

    private BoxCollider2D bc; // Ref is BoxCollider2D player
    private Rigidbody2D rb; // Ref to Rigidbody2D player
    private Animator an; // Ref for Animator GameObject Body
    private bool shooting; // The player this shooting?
    private float timeShooting; // Time that the player is shooting
    private Vector2 shootDirection; // Ref to standard Vector2 pointing in the direction of our player shot

    public GameObject shootingEffect; // Ref to GameObject containing particle effect player throwing
    public Transform gunTransform; // Ref to Transform the GameObject Gun (Gun contains sprite gun and aim)
    public Transform bodyTransform; // Ref to Transform the GameObject Body (Body contains the sprite of the body of the worm)
    public Transform bulletInitialTransform; // Ref to Transform guarding the initial position of the bullet

    private bool targetting; // The player is aiming?

    // Use this for initialization
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        // Looking for a component of type Animator in GameObjects children of Player
        // In fact we want the Animator component that is in GameObject Body
        an = GetComponentInChildren<Animator>();
        //gunTransform.eulerAngles = new Vector3 (0f, 0f, -30f);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))// The weapon becomes visible
        {
            targetting = true;
            gunTransform.gameObject.SetActive(true);
        }
        if (targetting)
        {
            UpdateTargetting();
            UpdateShootDetection();
            if (shooting)
                UpdateShooting();
        }
        //gun Transform.localEulerAngles = new Vector3 (0f, 0f, 30f);
    }

    // Checks if the player begins shooting
    void UpdateShootDetection()
    {
        // GetKeyDown // returns true only update when the player presses the button
        // GetKey returns true while the key is pressed
        // Returns true GetKeyUp the update on the player release the key
        if (Input.GetMouseButtonDown(0))
        {
            shooting = true;
            shootingEffect.SetActive(true);
            timeShooting = 0f;
        }
    }

    // If the player is shooting marks the qto time plyer this shooting and checks
    // If the Player stopped firing or has passed the threshold of shooting time
    // Tb calls the Shoot (), which effectively makes shooting
    void UpdateShooting()
    {
        timeShooting += Time.deltaTime;
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            shooting = false;
            shootingEffect.SetActive(false);
            Shoot();
        }
        if (timeShooting > maxTimeShooting)
        {
            shooting = false;
            shootingEffect.SetActive(false);
            Shoot();
        }
    }

    // Function that creates a GameObject Bullet from bulletPrefab
    // Positions created new bullet
    // And tb directs the direction in which the player is targeting:
    // Vector2 whose origin and destination the player the position of the mouse
    void Shoot()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        Vector2 playerToMouse = new Vector2(mousePosWorld.x - transform.position.x,
                                            mousePosWorld.y - transform.position.y);

        playerToMouse.Normalize();

        shootDirection = playerToMouse;
        Debug.Log("Shoot!");
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletInitialTransform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletMaxInitialVelocity * (timeShooting / maxTimeShooting);
    }

    // Upgrading the rotation of the gun and therefore the aim based on where the player is aiming
    // Tb must update the bodyTransform scale for the body of our Player comply with the direction in which the player is aiming
    void UpdateTargetting()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        Vector2 playerToMouse = new Vector2(mousePosWorld.x - transform.position.x,
                                            mousePosWorld.y - transform.position.y);

        playerToMouse.Normalize();

        float angle = Mathf.Asin(playerToMouse.y) * Mathf.Rad2Deg;
        if (playerToMouse.x < 0f)
            angle = 180 - angle;

        if (playerToMouse.x > 0f && bodyTransform.localScale.x > 0f)
        {
            bodyTransform.localScale = new Vector3(-bodyTransform.localScale.x, bodyTransform.localScale.y, 0f);
        }
        else if (playerToMouse.x < 0f && bodyTransform.localScale.x < 0f)
        {
            bodyTransform.localScale = new Vector3(-bodyTransform.localScale.x, bodyTransform.localScale.y, 0f);
        }

        gunTransform.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    // Update the speed of our Player relying on the keypresses
    void UpdateMove()
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            rb.velocity = Vector2.right * velocity;
            if (bodyTransform.localScale.x > 0f)
                bodyTransform.localScale = new Vector3(-bodyTransform.localScale.x, bodyTransform.localScale.y, 0f);

            an.SetBool("moving", true);
        }
        else if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            rb.velocity = -Vector2.right * velocity;
            if (bodyTransform.localScale.x < 0f)
                bodyTransform.localScale = new Vector3(-bodyTransform.localScale.x, bodyTransform.localScale.y, 0f);

            an.SetBool("moving", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            an.SetBool("moving", false);
        }
    }

    // Function call around frame in which there is collision between Collider Player and other Collider
    void OnCollisionStay2D(Collision2D other)
    {
        // So we updated the speed at x Player qdo this is not ground
        if (other.collider.tag == "Ground")
        {
            UpdateMove();
        }
    }
}
