using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionSpace : BaseBoardSpace {
    public BaseBoardSpace[] connected = { };

    public override void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace> cons) {
        if (n <= 0) {
            cons(this);
            return;
        }

        Debug.Log("prev: " + previous.name + " this: " + name + " n: " + n);
        foreach (BaseBoardSpace c in connected) {
            if (c == previous) continue;
            c.Next(this, n - 1, cons);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        foreach (BaseBoardSpace c in connected) {
            if (c is null) continue;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, c.transform.position);
        }
    }

    private void OnValidate() {
        foreach (BaseBoardSpace c in connected) {
            if (c is BoardSpace bc) {
                if (bc.spaceBefore is null) bc.spaceBefore = this;
                else if (bc.spaceBefore != this && (bc.spaceAfter is null)) bc.spaceAfter = this;
            }
        }
    }
#endif
}
