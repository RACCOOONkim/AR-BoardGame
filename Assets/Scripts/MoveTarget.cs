using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour {
    [SerializeField] private float showDuration = 0.1f;
    private BaseBoardSpace space;

    public void Set(BaseBoardSpace space) {
        transform.position = space.transform.position;
        this.space = space;

        transform.localScale = Vector3.zero;
        StartCoroutine(IShow());
    }

    public void Clicked() {
        GameManager.main.EndPlayerChoice();
        GameManager.main.AdvancePlayer(GameManager.main.CurrentPlayer(), space, true);
    }

    public void Hide() {
        StartCoroutine(IHide());
    }

    IEnumerator IShow() {
        float t = 0;
        while (t < showDuration) {
            transform.localScale = Vector3.one * t / showDuration;
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    IEnumerator IHide() {
        float t = 0;
        while (t < showDuration) {
            transform.localScale = Vector3.one * (1f - t / showDuration);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
