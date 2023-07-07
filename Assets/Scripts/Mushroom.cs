using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {
    public GameObject otherObject;

    private Rigidbody2D rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(1f, rigidbody.velocity.y);
    }

    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            otherObject.GetComponent<PlayerController>().PowerUp();
            Destroy(gameObject);
        }

        if (collision.contacts[0].normal.x == 1.0f)
        {
            transform.localScale = new Vector2(1, 1);
            rigidbody.velocity = new Vector2(1f, rigidbody.velocity.y);
        }
        else if (collision.contacts[0].normal.x == -1.0f)
        {
            transform.localScale = new Vector2(-1, 1);
            rigidbody.velocity = new Vector2(-1f, rigidbody.velocity.y);
        }

    }

}
