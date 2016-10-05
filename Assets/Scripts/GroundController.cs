using UnityEngine;
using System.Collections;

public class GroundController : MonoBehaviour
{

    private SpriteRenderer sr;
    private float widthWorld, heightWorld;
    private int widthPixel, heightPixel;
    private Color transp;

    private Transform bulletTf;

    // Start() de GroundController
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // sr: global variable Ground Controller ref to SpriteRenderer Ground
        Texture2D tex = (Texture2D)sr.sprite.texture;
        // Resources.Load ("filename") loads a file located
        // in Assets / Resources
        Texture2D tex_clone = Instantiate(tex);
        // We created a Texture2D tex clone to not change the original image 
        sr.sprite = Sprite.Create(tex_clone,
            new Rect(0f, 0f, tex_clone.width, tex_clone.height),
            new Vector2(0.5f, 0.5f), 100f);
        transp = new Color(0f, 0f, 0f, 0f);
        InitSpriteDimensions();
        bulletTf = gameObject.transform;
        //BulletController.groundController = this;
    }



    private void InitSpriteDimensions()
    {
        widthWorld = sr.bounds.size.x;
        heightWorld = sr.bounds.size.y;
        widthPixel = sr.sprite.texture.width;
        heightPixel = sr.sprite.texture.height;
    }

    void Update()
    {

    }

    public void DestroyGround(Collider2D cc)
    {
        // This creates a square this size.
        // Hardcoded for testing purpose.
        int explosionSize = 50;

        // Now we need an array to fit the changes.
        Color[] explosionColor = new Color[explosionSize * explosionSize];

        // Lets fill the array.
        for (int i = 0; i < explosionColor.Length; i++)
        {
            explosionColor[i] = new Color(1, 0, 0, 1);
        }

        // To see where the bullet is relative to me I
        // put the bullet inside of me.
        // Then use its local position - half its scale
        // to center the explosion point.
        // And take that and calculate the position
        // in pixels.
        bulletTf.parent = transform;
        V2int localPos = World2Pixel(bulletTf.position.x - (bulletTf.lossyScale.x / 2), bulletTf.position.y - (bulletTf.lossyScale.y / 2));

        // Set changes to the texture.
        sr.sprite.texture.SetPixels(localPos.x, localPos.y, explosionSize, explosionSize, explosionColor);

        // And apply them.
        sr.sprite.texture.Apply();

        // Reset collider mesh
        // there is probably a much better way to do this
        // performance wise. But this works if not
        // used too bad.
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        /*
        Debug.Log("he called: " + cc);

        V2int c = World2Pixel(cc.bounds.center.x, cc.bounds.center.y);
        // destruction of the circle center in pixels
        int r = Mathf.RoundToInt(cc.bounds.size.x * widthPixel / widthWorld);
        // r => destruction of the circle radius

        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - x * x));

            for (y = 0; y <= d; y++)
            {
                px = c.x + x;
                nx = c.x - x;
                py = c.y + y;
                ny = c.y - y;

                sr.sprite.texture.SetPixel(px, py, transp);
                sr.sprite.texture.SetPixel(nx, py, transp);
                sr.sprite.texture.SetPixel(px, ny, transp);
                sr.sprite.texture.SetPixel(nx, ny, transp);
            }
        }
        sr.sprite.texture.Apply();
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        */
    }

    private V2int World2Pixel(float x, float y)
    {
        V2int v = new V2int();

        float dx = x - transform.position.x;
        v.x = Mathf.RoundToInt(0.5f * widthPixel + dx * widthPixel / widthWorld);

        float dy = y - transform.position.y;
        v.y = Mathf.RoundToInt(0.5f * heightPixel + dy * heightPixel / heightWorld);

        return v;
    }
}
