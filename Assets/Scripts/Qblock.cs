using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qblock : MonoBehaviour {
    // flower, mushroom or star
    public GameObject itemPrefab;

    public AudioClip audioClip;
    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private Animator animator;

    private bool bHit = false;
    private float hitTime = 0.0f;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator SpawnItemPrefab(int d) {
        hitTime += Time.deltaTime;
        audioSource.clip = audioClip;
        audioSource.Play();

        yield return new WaitForSeconds(0.4f);

        Vector2 spawnPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject item = Instantiate(itemPrefab, spawnPosition, spawnRotation);

        Vector2 targetPosition = spawnPosition + Vector2.up * 0.16f;
        float elapsedTime = 0.0f;
        float duration = 1.0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;  // 보간 값 계산
            item.transform.position = Vector2.Lerp(spawnPosition, targetPosition, t);  // 보간 이동
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.transform.position = targetPosition;

        Item itemScript = item.GetComponent<Item>();
        itemScript.SetDirection(d);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && collision.contacts[0].normal.y == 1 && !bHit) {
            int direction = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            bHit = true;
            animator.SetTrigger("OnHit");
            StartCoroutine(SpawnItemPrefab(direction));
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (bHit && hitTime <= 0.4f) {
            if (collision.gameObject.tag == "Enemy" && collision.contacts[0].normal.y == -1) {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.OnDamaged();
            }
        }
    }
}
