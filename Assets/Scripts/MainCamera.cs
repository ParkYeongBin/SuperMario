using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    public GameObject target;
    public float smoothSpeed = 0.125f;
    public float maxY = 24f;
    public float minX = 0f; // ī�޶��� �ּ� x ��ǥ�� �����մϴ�.

    private float cameraHeight;

    void Start() {
        if (target == null) {
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        cameraHeight = Mathf.Clamp(transform.position.y, 0f, maxY);
    }

    void FixedUpdate() {
        Vector3 targetPosition = target.transform.position;

        // ī�޶��� x ��ǥ�� �ּҰ��� ���� �ʵ��� �����մϴ�.
        float clampedX = Mathf.Max(targetPosition.x, minX);

        Vector3 smoothedPosition = new Vector3(clampedX, cameraHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, smoothedPosition, smoothSpeed);
    }
}
