using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float jumpPower;
    public AudioClip jumpClip_1;
    public AudioClip jumpClip_2;
    public AudioClip powerUpClip;
    public AudioClip powerDownClip;

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private AudioSource playerAudio;

    private bool bFire = false, bSize = false;
    private bool bGround = false, bMove = false, bDown = false;
    private bool bAttack = false, bDie = false;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update() {
        Move();
        Jump();
        Down();
    }

    void Move() {
        if (Input.GetKey(KeyCode.A) && !bDown)
        {
            bMove = true;
            transform.localScale = new Vector2(-1, 1);
            playerRigidbody.velocity = new Vector2(-1f, playerRigidbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) && !bDown)
        {
            bMove = true;
            transform.localScale = new Vector2(1, 1);
            playerRigidbody.velocity = new Vector2(1f, playerRigidbody.velocity.y);
        }
        else
        {
            bMove = false;
        }

        playerAnimator.SetBool("bRun", bMove);
    }

    void Jump() {
        if(Input.GetKeyDown(KeyCode.W) && bGround)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            bGround = false;

            if (bSize)
                playerAudio.clip = jumpClip_2;
            else
                playerAudio.clip = jumpClip_1;

            playerAudio.Play();
        }

        playerAnimator.SetBool("bJump", !bGround);
    }

    void Down() {
        if (Input.GetKeyDown(KeyCode.S) && bSize)
            bDown = true;
        else if (Input.GetKeyUp(KeyCode.S) && bSize)
            bDown = false;

        playerAnimator.SetBool("bDown", bDown);
    }

    void OnAttack(Transform enemy) {
        
    }

    void Die() {
        // Die
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            bGround = true;
        }


        if (collision.gameObject.tag == "Enemy") {
            if (playerRigidbody.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                //Attack
            }
            else
                PowerDown();
        }
    }

    public void PowerUp() {
        bSize = true;
        playerAnimator.SetBool("bSize", bSize);

        // 이펙트 효과를 추가해야 함

        playerAudio.clip = powerUpClip;
        playerAudio.Play();
    }

    public void PowerDown() {
        if (bSize) {
            bSize = false;
            playerAnimator.SetBool("bSize", bSize);

            // 이펙트 효과 추가

            playerAudio.clip = powerDownClip;
            playerAudio.Play();
        }
        else
            Die();
    }
}
