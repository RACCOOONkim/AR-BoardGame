using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject movePrefab;

    public BaseBoardSpace currentSpace;

    public void Move(int n) {
        MoveConsumer(n, b => {

        });
    }

    private void MoveConsumer(int n, Action<BaseBoardSpace> cons) {
        currentSpace.Next(null, n, cons);
    }
}
