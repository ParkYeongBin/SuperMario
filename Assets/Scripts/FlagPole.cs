using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPole : MonoBehaviour {
    public float desc;

    private float distance;
    private Transform flag;

	void Start() {
		flag = transform.Find("Flag");
	}

    private IEnumerator FlagDown() {
        Vector2 startPosition = flag.position;
        Vector2 targetPosition = new Vector2(flag.position.x, flag.position.y - distance);

        float duration = 1.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            flag.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        flag.position = targetPosition;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameManager.instance.audioSource.clip = GameManager.instance.endingClip;
            GameManager.instance.audioSource.Play();
            this.transform.GetComponent<BoxCollider2D>().enabled = false;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            distance = (player.gameObject.transform.position.y - desc);
            StartCoroutine(FlagDown());
        }
    }
}
