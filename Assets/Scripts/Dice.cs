using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {
    private const float THRESH = 0.707f;
    [SerializeField] private int[] faces = {1, 2, 3, 4, 5, 6};

    [SerializeField] private Rigidbody rigid;
    public Vector3 startPosition = Vector3.zero;
    public GameObject puffFx, throwFx;

    [Header("Roll Dice")]
    public float throwForce = 5f;
    public float throwSkew = 0.2f;
    public float throwTorque = 5f;

    private bool rolling = false;
    private int result = 1;

    void Start() {
        //todo
    }

    void Update() {
        if(rolling && IsStatic()) {
            result = faces[CalculateResultFromAngle() - 1];
            rolling = false;

            Debug.Log(result);
        }
    }

    public void Roll() {
        Instantiate(puffFx, transform.position, Quaternion.identity);
        rigid.position = startPosition;
        rigid.rotation = Random.rotation;
        Instantiate(throwFx, startPosition, Quaternion.identity);
        rigid.velocity = new Vector3(Random.Range(-throwSkew, throwSkew), 1, Random.Range(-throwSkew, throwSkew)) * throwForce;
        rigid.angularVelocity = new Vector3(Random.Range(-throwTorque, throwTorque), throwTorque, Random.Range(-throwTorque, throwTorque));

        rolling = true;
    }

    private bool IsStatic() {
        return rigid.velocity.sqrMagnitude < 0.000001f && rigid.angularVelocity.sqrMagnitude < 0.000001f;
    }

    private int CalculateResultFromAngle() {
        if (transform.forward.y > THRESH) return 1;
        if (transform.up.y > THRESH) return 2;
        if(transform.right.y > THRESH) return 3;
        if(transform.forward.y < -THRESH) return 4;
        if(transform.up.y < -THRESH) return 5;
        if(transform.right.y < -THRESH) return 6;

        return 0;
    }

    public bool IsRolling() {
        return rolling;
    }

    public int GetResult() {
        if (rolling) return -1;
        return result;
    }
}
