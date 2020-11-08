using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour {
    public MeshRenderer[] hearts;
    public Explosion explosionPrefab;
    private int nextToDestroy = 0;


    public int SubtractOne() {
        MeshRenderer heartToDestroy = hearts[nextToDestroy];
        Instantiate(explosionPrefab, heartToDestroy.transform.position, new Quaternion());
        Destroy(heartToDestroy.gameObject);
        nextToDestroy += 1;
        return hearts.Length - nextToDestroy;
    }
}
