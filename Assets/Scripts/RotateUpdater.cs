using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUpdater : MonoBehaviour {
    public float speed = 60f;

    void Update() {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
