using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// how many times player can get hit before game stops
    /// </summary>
    public int lives = 4;

    /// <summary>
    /// firing angle
    /// </summary>
    public float FireAngle = Mathf.PI / 4;

    /// <summary>
    /// power of projectile
    /// </summary>
    public float FirePower = 5;
    
    /// <summary>
    /// speed of angle/power adjustment
    /// </summary>
    public float PowerAdjustmentSpeed = 0.1f;
    public float AngleAdjustmentSpeed = 0.01f;

    /// <summary>
    /// constaints for angle and power
    /// </summary>
    private Vector2 AngleMinMax = new Vector2(0, Mathf.PI / 2);
    private Vector2 PowerMinMax = new Vector2(5, 15);

    /// <summary>
    /// CD for shots
    /// </summary>
    public float CoolDownTime = 0.1f;
    private float coolDownCount;

    /// <summary>
    /// Prefab for bomb
    /// </summary>
    public GameObject BombPrefab;

    /// <summary>
    /// prefab for making trajecctory line
    /// </summary>
    public GameObject PointPrefab;
    public GameObject[] Points;
    public int numOfPoints = 20;

    public SpriteRenderer heart1;
    public SpriteRenderer heart2;
    public Sprite full;
    public Sprite half;
    public Sprite empty;

    public static Player Singleton;

    public AudioSource audioSource;
    public AudioClip damageSound;

    public static void ResetLives()
    {
        Singleton.lives = 4;
        Singleton.heart1.sprite = Singleton.full;
        Singleton.heart2.sprite = Singleton.full;
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        coolDownCount = Time.time;
        Points = new GameObject[numOfPoints];
        for (int i = 0; i < numOfPoints; i++)
        {
            Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("space") && !GameManager.isGameOver())
        {
            if (Time.time > coolDownCount)
            {
                Fire();
                coolDownCount = Time.time + CoolDownTime;
            }
        }

        for (int i = 0; i < numOfPoints; i++)
        {
            Points[i].transform.position = getPointPosition(i);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag != "Bomb")
        {
            lives -= 1;
            audioSource.PlayOneShot(damageSound);
            if (lives == 3)
            {
                heart2.sprite = half;
            }
            else if (lives == 2)
            {
                heart2.sprite = empty;
                heart1.sprite = full;
            }
            else if (lives == 1)
            {
                heart1.sprite = half;
            }
            else if (lives == 0)
            {
                heart1.sprite = empty;
                GameManager.setGameOver();
            }
        }
        
    }

    /// <summary>
    /// fires a bomb
    /// </summary>
    private void Fire()
    {
        GameObject bomb = Instantiate(BombPrefab, transform.position + transform.right, Quaternion.identity);
        Vector2 velocity = new Vector2(FirePower * Mathf.Cos(FireAngle), FirePower * Mathf.Sin(FireAngle));
        bomb.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void FixedUpdate()
    {

        if (Input.GetKey("right") && FireAngle > AngleMinMax.x + AngleAdjustmentSpeed)
        {
            FireAngle -= AngleAdjustmentSpeed;
        }
        else if (Input.GetKey("left") && FireAngle < AngleMinMax.y -  AngleAdjustmentSpeed)
        {
            FireAngle += AngleAdjustmentSpeed;
        }

        if (Input.GetKey("up") && FirePower < PowerMinMax.y - PowerAdjustmentSpeed)
        {
            FirePower += PowerAdjustmentSpeed;
        }
        else if (Input.GetKey("down") && FirePower > PowerMinMax.x + PowerAdjustmentSpeed)
        {
            FirePower -= PowerAdjustmentSpeed;
        }
    }

    /// <summary>
    /// calculate position for each point for some t
    /// </summary>
    Vector2 getPointPosition(int i)
    {
        float vx = FirePower * Mathf.Cos(FireAngle);
        float vy = FirePower * Mathf.Sin(FireAngle);
        float g = Physics2D.gravity.y;
        float range = vx * vy / g;
        float t = - 2 * (float) i / (float) numOfPoints * range / vx;
        float x = vx * t;
        float y = vy * t + 0.5f * g * t*t;
        Vector3 pos = new Vector3(x, y, 0);
        return transform.position + transform.right + pos;
    }
}
