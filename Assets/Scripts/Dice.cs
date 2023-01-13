using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {
    private const float THRESH = 0.707f;
    [SerializeField] private int[] faces = {1, 2, 3, 4, 5, 6};

    [SerializeField] private Rigidbody rigid;
    public Vector3 startPosition = Vector3.zero;

    [Header("Roll Dice")]
    public float throwForce = 5f;
    public float throwSkew = 0.2f;
    public float throwTorque = 5f;

    private bool rolling = false;
    private int result = 1;

    void Start() {

    }

    void Update() {
        //todo don't do this
        if (Input.GetMouseButtonDown(1)) Roll();

        if(rolling && IsStatic()) {
            result = faces[CalculateResultFromAngle() - 1];
            rolling = false;

            Debug.Log(result);
        }
    }

    public void Roll() {
        rigid.position = startPosition;
        rigid.rotation = Random.rotation;
        rigid.velocity = new Vector3(Random.Range(-throwSkew, throwSkew), 1, Random.Range(-throwSkew, throwSkew)) * throwForce;
        rigid.angularVelocity = new Vector3(Random.Range(-throwTorque, throwTorque), throwTorque, Random.Range(-throwTorque, throwTorque));

        rolling = true;
    }

    private bool IsStatic() {
        return rigid.velocity.sqrMagnitude < 0.000001f && rigid.angularVelocity.sqrMagnitude < 0.000001f;
    }

    private int CalculateResultFromAngle() {
        Vector3 f = transform.forward;
        if (f.x > THRESH) return 1;
        if (f.y > THRESH) return 2;
        if(f.z > THRESH) return 3;
        if(f.x < -THRESH) return 4;
        if(f.y < -THRESH) return 5;
        if(f.z < -THRESH) return 6;

        return 1;
    }

    public bool IsRolling() {
        return rolling;
    }

    public int GetResult() {
        if (rolling) return -1;
        return result;
    }
}
