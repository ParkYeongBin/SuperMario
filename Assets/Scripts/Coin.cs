using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void Off() {
        GetComponent<CapsuleCollider2D>().enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.0f);
    }

    public void PopUpCoin() {
        animator.SetTrigger("PopUp");
        audioSource.Play();
        GameManager.instance.AddScore(200);
        GameManager.instance.AddCoinCount(1);
        Destroy(gameObject, 1.0f);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Off();
            audioSource.Play();
            GameManager.instance.AddScore(200);
            GameManager.instance.AddCoinCount(1);
            Destroy(gameObject, 1.0f);
        }
    }
}
