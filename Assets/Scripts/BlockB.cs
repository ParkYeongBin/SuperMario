using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockB : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject[] dummyPrefab = new GameObject[4];

    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private Animator animator;

    public int count;
    public bool coinMode;
    private bool onHit = false;
    private bool isActivated = true;
    

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator UpAndDown() {
        float elapsedTime = 0.0f;
        float duration = 0.1f;

        Vector2 currentPos = new Vector2(rigidbody.position.x, rigidbody.position.y);
        Vector2 targetPos1 = currentPos + Vector2.up * 0.08f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(transform.position, targetPos1, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(transform.position, currentPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SpawnCoinPrefab() {
        audioSource.Play();

        onHit = true;
        yield return UpAndDown();
        onHit = false;

        Vector2 spawnPosition = new Vector2(rigidbody.position.x, rigidbody.position.y + 0.16f);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject item = Instantiate(coinPrefab, spawnPosition, spawnRotation);
        item.gameObject.GetComponent<Coin>().PopUpCoin();

        Vector2 targetPosition = spawnPosition + Vector2.up * 0.64f;
        float elapsedTime = 0.0f;
        float duration = 0.2f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            item.transform.position = Vector2.Lerp(spawnPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            item.transform.position = Vector2.Lerp(targetPosition, spawnPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.gameObject.GetComponent<Coin>().Off();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(isActivated && collision.gameObject.tag == "Player" && collision.contacts[0].normal.y == 1) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.GetBsize()) {
                if (coinMode) {
                    StartCoroutine(SpawnCoinPrefab());
                    count--;
                    if (count <= 0) {
                        animator.SetTrigger("Deactive");
                        isActivated = false;
                    }
                }
                else {
                    Vector2 spawnPosition = rigidbody.position;
                    Quaternion spawnRotation = Quaternion.identity;
                    GameObject[] dummies = new GameObject[4];
                    for (int i = 0; i < 4; i++) {
                        dummies[i] = Instantiate(dummyPrefab[i], spawnPosition, spawnRotation);
                        dummies[i].gameObject.GetComponent<Dummy>().Scattered();
                    }

                    Destroy(gameObject);
                }
            }
            else {
                audioSource.Play();
                StartCoroutine(UpAndDown());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (isActivated && onHit && collision.contacts[0].normal.y == -1) {
            if (collision.gameObject.tag == "Enemy") {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.OnDamaged();
            }
            else if (collision.gameObject.tag == "Item") {
                Item item = collision.gameObject.GetComponent<Item>();
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3, 0.5f) * 1, ForceMode2D.Impulse);
                item.SetDirection(-1);
            }
        }
    }
}
