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
        if (rigidbody.position.y < -1)
            Destroy(gameObject);

        rigidbody.velocity = new Vector2(0.3f * direction, rigidbody.velocity.y);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player" &&
            (collision.contacts[0].normal.x == -1 || collision.contacts[0].normal.x == 1) ) {
            direction *= -1;
            Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2.0f);
        }
    }

    public int GetDirection() { return direction; }
    public void SetDirection(int d) { direction = d; }
}
