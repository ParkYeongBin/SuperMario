using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    public GameObject[] gameObjects;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            for (int i = 0; i < gameObjects.Length; i++) 
                gameObjects[i].GetComponent<Enemy>().Activate();

            Destroy(gameObject);
        }
    }
}
