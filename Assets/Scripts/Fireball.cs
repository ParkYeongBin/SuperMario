using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private int direction;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        rigidbody.AddForce(Vector2.right * direction, ForceMode2D.Impulse);

    }

    void Update() {

        Destroy(gameObject, 3f);
    }

    public void SetDirection(int d) { direction = d; }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1 || collision.contacts[0].normal.x == -1) {
            if (collision.gameObject.tag == "Enemy") {
                Enemy enemies = collision.gameObject.GetComponent<Enemy>();
                enemies.OnDamaged();
            }
            else {
                direction *= -1;
            }
        }
    }
}
