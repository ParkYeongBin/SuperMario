using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {
    public int mode;
    public float gravity = -9.8f;

    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();

        if (mode == 1)
            audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (rigidbody.position.y < -0.5f)
            Destroy(gameObject);
    }

    public void Scattered()  {
        rigidbody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);

        switch (mode) {
            case 1:
                audioSource.Play();
                rigidbody.AddForce(Vector2.right * 1, ForceMode2D.Impulse);
                break;
            case 2:
                rigidbody.AddForce(Vector2.right * 1.5f, ForceMode2D.Impulse);
                break;
            case 3:
                rigidbody.AddForce(Vector2.left * 1, ForceMode2D.Impulse);
                break;
            case 4:
                rigidbody.AddForce(Vector2.left * 1.5f, ForceMode2D.Impulse);
                break;
        }
    }
}
