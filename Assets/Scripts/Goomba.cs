using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnDamaged() {
        if (isDead)
            return;

        audioSource.Play();

        isDead = true;

        animator.SetTrigger("Death");

        Invoke("OnDamaged", 0.3f);

        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        //Collider Disable
        circleCollider.enabled = false;

        //Die Effect Jump
        rigidbody.AddForce(Vector2.up * 50);

        Destroy(gameObject, 1);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);
    }
}

