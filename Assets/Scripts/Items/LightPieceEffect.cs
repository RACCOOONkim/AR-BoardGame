using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPieceEffect : MonoBehaviour {
    [SerializeField] private GameObject pivot;
    [SerializeField] private ParticleSystem[] pss;

    public float duration = 1f, endDuration = 1f;
    public float jumpHeight = 3f;

    IEnumerator Start() {
        transform.Rotate(Random.Range(0, 20f), 0, Random.Range(0, 30f));

        float f = 0;
        while (f < 1f) {
            pivot.transform.localPosition = Vector3.up * f * (1 - f) * jumpHeight * 4f;
            f += Time.deltaTime / duration;
            yield return null;
        }

        foreach (ParticleSystem ps in pss) {
            ps.Stop();
        }

        yield return new WaitForSeconds(endDuration);
        Destroy(gameObject);
    }
}
