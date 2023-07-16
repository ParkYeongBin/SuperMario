using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject coinPrefab;

    public int count;
    public int mode;
    public AudioClip audioClip;
    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.GetBsize()) {

            }
            else {

            }
        }
    }
}
