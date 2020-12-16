using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMove : MonoBehaviour
{
    public Explosion explosionPrefab;
    public float hoverRange = 2f;
    public float hoverSpeed = 2f;
    public float rotateSpeed = 0.2f;

    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      transform.position = _startPosition + new Vector3(0.0f, hoverRange * Mathf.Sin(Time.time * hoverSpeed), 0.0f);
      transform.Rotate(rotateSpeed, rotateSpeed, rotateSpeed, Space.Self);
    }

    public void HideSelfAndExplode() {
        gameObject.SetActive(false);
        Explode();
    }

    public void DestroySelf(bool explode) {
        Explode();
        Destroy(gameObject);
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
    }
}
