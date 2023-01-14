using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float walkDuration = 0.2f, walkBounce = 0.2f;
    [SerializeField] private float flyDuration = 0.7f, flyBounce = 1f;
    [SerializeField] private TrailRenderer trail;

    public BaseBoardSpace currentSpace;
    public int lightPiece, darkPiece;

    public List<Item> items = new List<Item>();
    private List<BaseBoardSpace> tmpHistory = new List<BaseBoardSpace>();

    public void Init() {
        lightPiece = darkPiece = 0;
    }

    public void MoveConsumer(int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons) {
        tmpHistory.Clear();
        currentSpace.Next(null, n, cons, tmpHistory);
    }

    public void WalkTo(List<BaseBoardSpace> history, Action next) {
        StartCoroutine(IWalk(history, next));
    }

    public void FlyTo(BaseBoardSpace target, Action next) {
        StartCoroutine(IFly(currentSpace, target, next));
    }

    IEnumerator IWalk(List<BaseBoardSpace> history, Action next) {
        int i = 0;
        while (i < history.Count - 1) {
            BaseBoardSpace c = history[i];
            BaseBoardSpace n = history[i + 1];
            float f = 0;
            while(f < 1) {
                float h = f * (1 - f) * walkBounce * 4;
                transform.position = Vector3.Lerp(c.transform.position, n.transform.position, f) + Vector3.up * h + offset;

                f += Time.deltaTime / walkDuration;
                yield return null;
            }
            transform.position = n.transform.position + offset;
            i++;
        }
        SetSpace(history[history.Count - 1]);
        next();
    }

    IEnumerator IFly(BaseBoardSpace c, BaseBoardSpace n, Action next) {
        float f = 0;
        while (f < 1) {
            float h = f * (1 - f) * flyBounce * 4;
            transform.position = Vector3.Lerp(c.transform.position, n.transform.position, f) + Vector3.up * h + offset;

            f += Time.deltaTime / flyDuration;
            yield return null;
        }
        SetSpace(n);
        next();
    }

    public void SetSpace(BaseBoardSpace space) {
        currentSpace = space;
        transform.position = space.transform.position + offset;
        transform.SetParent(space.transform);
    }

    public Vector3 TrailPos(float f) {
        if(trail.positionCount == 0) return transform.position;
        f = 1 - f;
        return trail.GetPosition(Mathf.Min(Mathf.FloorToInt(trail.positionCount * f), trail.positionCount - 1));
    }
}
