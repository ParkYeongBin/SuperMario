using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected new Rigidbody2D rigidbody;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D colliderD;
    protected AudioSource audioSource;

    protected int movedirection;
    protected bool isDead = false;

    public virtual void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderD = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        Think();
    }

    // Update is called once per frame
    public virtual void Update() {
        rigidbody.velocity = new Vector2(movedirection * 0.3f, rigidbody.velocity.y);

        Vector2 frontVec = new Vector2(rigidbody.position.x + movedirection * 0.1f, rigidbody.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null && !isDead)
        {
            Debug.Log("No Platform there...!");
            movedirection *= -1;
            CancelInvoke("Think");
            Think();
        }
    }

    protected void Think() {
        Invoke("Think", 3);

        movedirection = Random.Range(0, 2);
        movedirection = (movedirection > 0) ? 1 : -1;
    }

    public virtual void OnDamaged() { /* contents */ }
}