using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private int movedirection;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        Think();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(movedirection * 0.3f, rigidbody.velocity.y);

        Vector2 frontVec = new Vector2(rigidbody.position.x + movedirection * 0.1f, rigidbody.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) {
            Debug.Log("No Platform there...!");
            movedirection *= -1;
            CancelInvoke();
            Think();
        }
    }

    private void Think()
    {
        Invoke("Think", 3);

        movedirection = Random.Range(0, 2);
        movedirection = (movedirection > 0) ? 1 : -1;
    }

    public void OnDamaged()
    {
        
    }
}
