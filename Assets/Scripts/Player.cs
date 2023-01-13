using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private Vector3 offset = Vector3.zero;
    public BaseBoardSpace currentSpace;

    public void MoveConsumer(int n, Action<BaseBoardSpace> cons) {
        currentSpace.Next(null, n, cons);
    }

    public void SetSpace(BaseBoardSpace space) {
        currentSpace = space;
        transform.position = space.transform.position + offset;
        transform.SetParent(space.transform);
    }
}
