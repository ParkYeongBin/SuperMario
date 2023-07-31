using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private int direction;
    public float maxBounceHeight;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        direction = FindObjectOfType<PlayerController>().GetDirection();
        rigidbody.velocity = new Vector2(direction * 2f, 0f);
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemies = collision.gameObject.GetComponent<Enemy>();
            enemies.OnDamaged();
            Destroy(gameObject);
        }
        else if (collision.contacts[0].normal.y > 0.6f)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 2f);
    }
}