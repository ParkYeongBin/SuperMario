using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    protected new Rigidbody2D rigidbody;
    private int direction;

    public virtual void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Update() {
        rigidbody.velocity = new Vector2(0.3f * direction, rigidbody.velocity.y);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player" &&
            (collision.contacts[0].normal.x == -1 || collision.contacts[0].normal.x == 1) ) {
            direction *= -1;
        }

        //if (collision.gameObject.CompareTag("DeathLine"))
            //Destroy(gameObject);
    }

    public int GetDirection() { return direction; }
    public void SetDirection(int d) { direction = d; }
}
