using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public Vector3 velocity;
    public float speed = 5f;
    private int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        targetIndex = 1;
    }

    void OldFixedUpdate() {
        Vector3 targetPosition = points[targetIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            transform.position = targetPosition;
            targetIndex += 1;
            if (targetIndex >= points.Length) {
                targetIndex = 0;
            }
        }
    }

    void FixedUpdate(){
        Vector3 targetPosition = points[targetIndex].position;
        MoveTowardsTarget(targetPosition);
        ChangeTargetIfReached(targetPosition);
    }

    void MoveTowardsTarget(Vector3 targetPosition) {
        Vector3 direction = Vector3.ClampMagnitude(targetPosition - transform.position, 1f);
        velocity = direction * speed * Time.deltaTime;
        transform.Translate(velocity);
    }

    void ChangeTargetIfReached(Vector3 targetPosition) {
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            transform.position = targetPosition;
            targetIndex += 1;
            if (targetIndex >= points.Length) {
                targetIndex = 0;
            }
        }
    }
}
