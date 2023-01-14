using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : BaseBoardSpace {
    public BaseBoardSpace spaceBefore, spaceAfter;

    public override void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons, List<BaseBoardSpace> history) {
        history.Add(this);
        if (n <= 0) {
            cons(this, history);
            return;
        }

        if (spaceBefore == previous && (spaceAfter is not null) && (previous is not null)) spaceAfter.Next(this, n - 1, cons, history);
        else if(spaceAfter == previous && (spaceBefore is not null) && (previous is not null)) spaceBefore.Next(this, n - 1, cons, history);
        else {
            //one-way merge
            var newHistory = new List<BaseBoardSpace>(history);
            if (spaceAfter is not null) spaceAfter.Next(this, n - 1, cons, history);
            if (spaceBefore is not null) spaceBefore.Next(this, n - 1, cons, newHistory);
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
        if (mission is not null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, 0.2f);
        }
    }

    private void OnValidate() {
        if(spaceBefore is BoardSpace b && (b.spaceAfter is null)) {
            b.spaceAfter = this;
        }
        if (spaceAfter is BoardSpace a && (a.spaceBefore is null)) {
            a.spaceBefore = this;
        }

        if(spaceBefore == this) {
            Debug.LogError("OH GOD OH FUCK");
            spaceBefore = null;
        }
        if (spaceAfter == this) {
            Debug.LogError("OH GOD OH FUCK");
            spaceAfter = null;
        }
    }
#endif
}
