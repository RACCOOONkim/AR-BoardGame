using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEdgeSpace : BaseBoardSpace {
    public BaseBoardSpace spaceBefore;
    public BaseBoardSpace spaceMars, spaceUranus;
    public bool onLeftBoard = true;

    public override void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons, List<BaseBoardSpace> history) {
        history.Add(this);
        if (n <= 0) {
            cons(this, history);
            return;
        }

        BaseBoardSpace spaceAfter = SpaceAfter();

        if (spaceBefore == previous && (spaceAfter is not null) && (previous is not null)) spaceAfter.Next(this, n - 1, cons, history);
        else if (spaceAfter == previous && (spaceBefore is not null) && (previous is not null)) spaceBefore.Next(this, n - 1, cons, history);
        else {
            //one-way merge
            var newHistory = new List<BaseBoardSpace>(history);
            if (spaceAfter is not null) spaceAfter.Next(this, n - 1, cons, history);
            if (spaceBefore is not null) spaceBefore.Next(this, n - 1, cons, newHistory);
        }
    }

    private BaseBoardSpace SpaceAfter() {
        if(onLeftBoard) return GameManager.main.rightFlipped ? spaceUranus : spaceMars;
        else return GameManager.main.leftFlipped ? spaceUranus : spaceMars;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (spaceBefore is not null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceBefore.transform.position);
        }
        if (spaceMars is not null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceMars.transform.position);
        }
        if (spaceUranus is not null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, spaceUranus.transform.position);
        }
        if (mission is not null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, 0.2f);
        }
    }
#endif
}
