using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewaySpace : BaseBoardSpace {
    public BaseBoardSpace spaceAfter;

    public override void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons, List<BaseBoardSpace> history) {
        history.Add(this);
        if (n <= 0) {
            cons(this, history);
            return;
        }

        
        spaceAfter.Next(this, n - 1, cons, history);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (spaceAfter is not null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceAfter.transform.position);
        }
        if (mission is not null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, 0.2f);
        }
    }
#endif
}
