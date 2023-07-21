using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Off() {
        GetComponent<BoxCollider2D>().enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.0f);
    }

    public void PopUpCoin() {
        animator.SetTrigger("PopUp");
        audioSource.Play();
        //GameManager.instance.AddScore(200);
        // UI 200 점 추가
        Destroy(gameObject, 1.0f);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Off();
            audioSource.Play();
            //GameManager.instance.AddScore(200);
            // UI 200 점 추가
            Destroy(gameObject, 1.0f);
        }
    }
}
