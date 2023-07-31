using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaTroopa : Enemy {
    private CapsuleCollider2D capsuleCollider;
    private bool shellMode = false;
    private bool isMove = true;
    private float duration = 0f;

    public override void Start() {
        base.Start();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public override void Update() {
        transform.localScale = new Vector2(movedirection * -1, 1);

        if (rigidbody.position.y < -1)
            Destroy(gameObject);

        if (shellMode) {
            duration += Time.deltaTime;

            if (isMove) {
                rigidbody.velocity = new Vector2(movedirection * 1.0f, rigidbody.velocity.y);
                duration = 0;
            }
            else {
                if (duration > 10.0f) {
                    animator.SetTrigger("On2");
                    shellMode = false;
                    isMove = true;
                    duration = 0f;
                }
            }
        }
        else if(!shellMode) {
            rigidbody.velocity = new Vector2(movedirection * 0.3f, rigidbody.velocity.y);
            duration = 0;
        }
    }

    public override void OnDamaged() {
        if(!shellMode) {
            audioSource.Play();
            shellMode = true;
            isMove = false;
            animator.SetTrigger("On");
            gameObject.layer = 9;
            StartCoroutine(Delay(0.2f));
            gameObject.layer = 7;
        }
        else {
            duration = 0;
        }
    }

    public void OnDamaged2() {
        if (isDead)
            return;

        audioSource.Play();

        isDead = true;

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
        if (shellMode && isMove)
        {
            duration = 0f;

            if (collision.gameObject.tag == "Player" &&
                (collision.contacts[0].normal.x == -1 || collision.contacts[0].normal.x == 1))
            {
                collision.gameObject.GetComponent<PlayerController>().PowerDown(rigidbody.transform.position);
                isMove = false;
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<Enemy>().OnDamaged();
                isMove = false;
            }
        }
        else if (shellMode && !isMove)
        {
            if (collision.gameObject.tag == "Player")
            {
                gameObject.layer = 9;
                audioSource.Play();
                duration = 0f;
                movedirection = collision.gameObject.GetComponent<PlayerController>().GetDirection();
                isMove = true;
                StartCoroutine(Delay(0.5f));
                gameObject.layer = 7;
            }
        }
        else
            if (collision.gameObject.tag == "Enemy")
            movedirection *= -1;
    }

    private IEnumerator Delay(float d) {
        yield return new WaitForSecondsRealtime(d);
    }
}
