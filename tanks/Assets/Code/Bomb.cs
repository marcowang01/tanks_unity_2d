using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float CoolDownTime = 0.1f;
    private float coolDownCount = 0;
    public Sprite BombSprite1;
    public Sprite BombSprite2;
    public bool currentSprite;

    public GameObject ExplosionPrefab;

    private void FixedUpdate()
    {
        if (Time.time > coolDownCount)
        {
            if (currentSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BombSprite1;
            } else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BombSprite2;
            }

            currentSprite = !currentSprite;
            coolDownCount = Time.time + CoolDownTime;
        }
        
    }

    /// <summary>
    /// If this gets called, then we're off screen
    /// So destroy ourselves
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        if (transform.position.x > 9 || transform.position.x < -9)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// destroy bomb on collision
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag != "Player" &&
                collision.collider.gameObject.tag != "Bomb" && gameObject)
        {
            GetComponent<PointEffector2D>().enabled = true;
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity,
            transform.parent);
            Destroy(gameObject);
        }
    }
}
