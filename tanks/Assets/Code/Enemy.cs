using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Torque power
    /// </summary>
    public float WheelPower;

    /// <summary>
    /// how fast the enemy is launch on contact w bomb
    /// </summary>
    public float LaunchFactor;

    /// <summary>
    /// starting wheel power
    /// </summary>
    public float InitWheelPower = 0.5f; 
    private Rigidbody2D rb;

    /// <summary>
    /// Initialize player and rigidBody fields
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        WheelPower = InitWheelPower + ScoreKeeper.getPoints() * 0.25f;
        if (GameManager.isGameOver())
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.AddTorque(WheelPower);
    }

    /// <summary>
    /// destroys object if offscreen
    /// </summary>
    void OnBecameInvisible()
    {
        if (gameObject)
        {
            Destroy(gameObject);
        }   
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player" && gameObject)
        {
            Destroy(gameObject);
        }

        if (collision.collider.gameObject.tag == "Bomb")
        { 
            ScoreKeeper.ScorePoints(1);
            Vector2 temp = collision.collider.gameObject.GetComponent<Rigidbody2D>().velocity;
            temp.y = -temp.y;
            rb.velocity = temp * 10;
        }
    }
}
