using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : BaseBoardSpace {
    public BaseBoardSpace spaceBefore, spaceAfter;

    public override void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace> cons) {
        if (n <= 0) cons(this);

        if (spaceBefore == previous && (spaceAfter is not null)) spaceAfter.Next(this, n - 1, cons);
        else if(spaceAfter == previous && (spaceBefore is not null)) spaceBefore.Next(this, n - 1, cons);
        else {
            //one-way merge
            if (spaceAfter is not null) spaceAfter.Next(this, n - 1, cons);
            if (spaceBefore is not null) spaceBefore.Next(this, n - 1, cons);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (spaceBefore is not null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceBefore.transform.position);
        }
        if (spaceAfter is not null) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceAfter.transform.position);
        }
    }

    private void OnValidate() {
        if(spaceBefore is BoardSpace b && (b.spaceAfter is null)) {
            b.spaceAfter = this;
        }
        if (spaceAfter is BoardSpace a && (a.spaceBefore is null)) {
            a.spaceBefore = this;
        }
    }
#endif
}
