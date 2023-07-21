using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockQ : MonoBehaviour {
    public GameObject itemPrefab;

    public AudioClip audioClip;
    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private Animator animator;

    private bool isActivated = false;
    private bool onHit = false;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator SpawnItemPrefab(int d) {
        audioSource.clip = audioClip;
        audioSource.Play();

        onHit = true;
        yield return UpAndDown();
        onHit = false;

        Vector2 spawnPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject item = Instantiate(itemPrefab, spawnPosition, spawnRotation);

        Vector2 targetPosition = spawnPosition + Vector2.up * 0.16f;
        float elapsedTime = 0.0f;
        float duration = 1.0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            item.transform.position = Vector2.Lerp(spawnPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.transform.position = targetPosition;

        Item itemScript = item.GetComponent<Item>();
        itemScript.SetDirection(d);
    }

    private IEnumerator UpAndDown() {
        float elapsedTime = 0.0f;
        float duration = 0.15f;

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && collision.contacts[0].normal.y == 1 && !isActivated) {
            int direction = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            isActivated = true;
            animator.SetTrigger("OnHit");
            StartCoroutine(SpawnItemPrefab(direction));
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
