using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject firePrefab;

    public AudioClip jumpClip_1;
    public AudioClip jumpClip_2;
    public AudioClip powerUpClip;
    public AudioClip powerDownClip;
    public AudioClip dieClip;

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private AudioSource playerAudio;
    private SpriteRenderer playerSpriteRenderer;

    public float jumpPower;
    private bool bFire = false, bSize = false;
    private bool bGround = false, bMove = false, bDown = false;
    private bool bAttack = false, bDie = false;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (bDie) 
            return;

        Move();
        Jump();
        Down();
        FireBall();
    }

    void Move() {
        if (Input.GetKey(KeyCode.A) && !bDown) {
            bMove = true;
            transform.localScale = new Vector2(-1, 1);
            playerRigidbody.velocity = new Vector2(-1f, playerRigidbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) && !bDown) {
            bMove = true;
            transform.localScale = new Vector2(1, 1);
            playerRigidbody.velocity = new Vector2(1f, playerRigidbody.velocity.y);
        }
        else {
            bMove = false;
        }

        playerAnimator.SetBool("bRun", bMove);
    }

    void Jump() {
        if(Input.GetKeyDown(KeyCode.W) && bGround) {
            bGround = false;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            playerAudio.clip = bSize ? jumpClip_2 : jumpClip_1;
            playerAudio.Play();
        }

        playerAnimator.SetBool("bJump", !bGround);
    }

    void Down() {
        if (!bSize) return;

        if (Input.GetKeyDown(KeyCode.S))
            bDown = true;
        else if (Input.GetKeyUp(KeyCode.S))
            bDown = false;

        playerAnimator.SetBool("bDown", bDown);
    }

    public void FireBall() {
        if (!bFire || !bSize) return;

        if (Input.GetKeyDown(KeyCode.L)) {
            GameObject gameObject = Instantiate(firePrefab, transform.position, transform.rotation);
            bAttack = true;
        }
        else
            bAttack = false;

        playerAnimator.SetBool("bAttack", bAttack);
    }

    public void Die() {
        bDie = true;
        playerAnimator.SetTrigger("Die");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        playerAudio.clip = dieClip;
        playerAudio.Play();

        playerRigidbody.velocity = new Vector2(0, 0);
        playerRigidbody.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

        StartCoroutine(PauseAndResume(0.5f));


        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f && !collision.gameObject.CompareTag("Enemy")) {
            bGround = true;
        }

        // 문제 발생: 측면 블럭에서 키 입력하면 공중에 떠 있음

        if (collision.gameObject.tag == "Enemy") {
            if (!bGround && playerRigidbody.velocity.y < 0 && transform.position.y > collision.transform.position.y) {
                OnAttack(collision.transform);
                playerRigidbody.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
            else {
                PowerDown(collision.transform.position);
            }

        }
    }

    public void PowerUp() {
        if (!bSize) {
            bSize = true;
            playerAnimator.SetBool("bSize", bSize);
            StartCoroutine(PauseAndResume(0.7f));
        }

        playerAudio.clip = powerUpClip;
        playerAudio.Play();
    }

    public void PowerDown(Vector2 target) {
        if (bSize) {
            bSize = false;
            playerAnimator.SetBool("bSize", bSize);

            StartCoroutine(PauseAndResume(0.4f));

            playerAudio.clip = powerDownClip;
            playerAudio.Play();

            StartCoroutine(OnDamaged(target));
        }
        else
            Die();
    }

    public void FireMode() {
        if (!bFire) {
            bFire = true;
            playerAnimator.SetBool("bFire", bFire);
            StartCoroutine(PauseAndResume(0.7f));
        }
        playerAudio.clip = powerUpClip;
        playerAudio.Play();
    }

    void OnAttack(Transform monster) {
        Enemy enemy = monster.GetComponent<Enemy>();
        enemy.OnDamaged();
    }

    public IEnumerator OnDamaged(Vector2 target) {
        gameObject.layer = 9;

        int direction = transform.position.x - target.x > 0 ? 1 : -1;
        playerRigidbody.AddForce(new Vector2(direction, 1) * 1, ForceMode2D.Impulse);

        yield return StartCoroutine(Blink());

        gameObject.layer = 8;
    }

    private IEnumerator Blink() {
        for (int i=0; i < 7; i++) {
            playerSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.1f);
            playerSpriteRenderer.color = new Color(1, 1, 1, 1.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool GetBsize() { return bSize; }
    public int GetDirection() {
        return transform.localScale.x > 0 ? 1 : -1;
    }
    public IEnumerator PauseAndResume(float delay) {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
    }
}
