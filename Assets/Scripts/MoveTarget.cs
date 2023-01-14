using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour {
    [SerializeField] private float showDuration = 0.1f;
    [SerializeField] private float hideDuration = 0.4f;
    [SerializeField] private ParticleSystem selectEffect, hideEffect;
    [SerializeField] private Collider col;

    private BaseBoardSpace space;
    private List<BaseBoardSpace> history;

    public void Set(BaseBoardSpace space, List<BaseBoardSpace> history) {
        transform.position = space.transform.position;
        this.space = space;
        this.history = history;

        transform.localScale = Vector3.one;
        //StartCoroutine(IShow());
    }

    public void Clicked() {
        GameManager.main.EndPlayerChoice();
        GameManager.main.AdvancePlayer(GameManager.main.CurrentPlayer(), space, true, history);

        selectEffect.Play();
    }

    public void Hide() {
        StartCoroutine(IHide());
    }

    /*IEnumerator IShow() {
        float t = 0;
        while (t < showDuration) {
            transform.localScale = Vector3.one * t / showDuration;
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }*/

    IEnumerator IHide() {
        col.enabled = false;
        foreach (ParticleSystem ps in transform.GetComponentsInChildren<ParticleSystem>()) {
            if (ps == selectEffect || ps == hideEffect) continue;
            ps.Stop();
            ps.Clear();
        }
        hideEffect.Play();

        /*float t = 0;
        while (t < hideDuration) {
            transform.localScale = Vector3.one * (1f - t / hideDuration);
            t += Time.deltaTime;
            yield return null;
        }*/
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
