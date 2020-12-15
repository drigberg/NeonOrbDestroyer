using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    public Arena arena;
    public Explosion explosionPrefab;
    public int glow = 25;
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

    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        if (sendExplodeArgs.attacking) {
            sendExplodeArgs.glow = glow;
            arena.Begin();
            DestroySelf(true);
        }
    }

    void DestroySelf(bool explode) {
        Explode();
        Destroy(gameObject);
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
    }
}
