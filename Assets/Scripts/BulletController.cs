using UnityEngine;
using System.Collections;

// Class responsible for the code that controls our bullet
public class BulletController : MonoBehaviour
{

    private Rigidbody2D rb;
    // Ref to Rigidbody2D our bullet
    public Transform bulletSpriteTransform;
    // Ref to transform the GameObject Sprite that is within GameObject Bullet
    private bool updateAngle = true;
    // bool that says whether or not to update the rotation GameObject Sprite based on Costume. bullet
    // Bool This is to say that after the bullet collides with some other body, rotation
    // The bullet should not be updated based on the trajectory
    public GameObject bulletSmoke;
    // Ref gameObject to Bullet Smoke containing particle system that makes the bullet trace
    public Collider2D destructionCircle;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateAngle)
        {
            Vector2 dir = new Vector2(rb.velocity.x, rb.velocity.y);
            // Determination of the velocity vector angle
            dir.Normalize();
            float angle = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            if (dir.x < 0f)
            {
                angle = 180 - angle;
            }

            //Debug.Log("angle = " + angle);

            // Updating the rotation Sprite (GameObject containing Sprite Render of our bullet) 
            // according to the angle of trajectory
            bulletSpriteTransform.localEulerAngles = new Vector3(0f, 0f, angle + 45f);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // When the bullet collides with another body that is not the player she
        // No longer updates the rotation based on the trajectory
        // And particle effect bullet trail is disabled
        if (coll.collider.tag == "Ground")
        {
            GroundController groundController = coll.gameObject.GetComponent<GroundController>();
            updateAngle = false;
            bulletSmoke.SetActive(false);
            groundController.DestroyGround(destructionCircle);
            Destroy(gameObject);
        }
    }
}
