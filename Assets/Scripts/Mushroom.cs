using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Item {

    public override void Start() {
        base.Start();
        SetDirection(1);
    }

    public override void Update() {
        base.Update();
    }

    public override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.PowerUp();
            GameManager.instance.AddScore(1000);
            Destroy(gameObject);
        }
    }
}
