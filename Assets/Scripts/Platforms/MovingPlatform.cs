using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;
    private int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        targetIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
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
}
