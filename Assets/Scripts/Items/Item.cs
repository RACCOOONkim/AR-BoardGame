using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public float animDuration = 1f, loseDuration = 1f, movementLerp = 30f;
    protected bool obtained = false;
    protected Player obtainedPlayer = null;

    protected virtual void Update() {
        if (obtained) {
            int id = obtainedPlayer.items.IndexOf(this);
            if (id != -1) {
                transform.position = Vector3.Lerp(transform.position, GetPreferredPos(id), movementLerp * Time.deltaTime);
            }
        }
    }

    private Vector3 GetPreferredPos(int id) {
        Vector3 pos = obtainedPlayer.TrailPos((id + 1) * 0.1f);
        if ((pos - obtainedPlayer.transform.position).sqrMagnitude < 0.01f) pos += (id + 2) * 0.7f * Vector3.up;
        return pos;
    }

    public void Place(BaseBoardSpace space) {
        space.item = this;
        transform.position = space.transform.position;
        transform.SetParent(space.transform);
        obtained = false;
        obtainedPlayer = null;
    }

    public void Get(Player player, Action next) {
        if (player.items.Contains(this)) {
            next();
            return;
        }
        player.items.Add(this);
        obtainedPlayer = player;
        transform.SetParent(null);
        OnObtain(player);
        StartCoroutine(IObtain(animDuration, next));
    }

    public void Lose(Player player, Action next) {
        if (!player.items.Remove(this)) {
            next();
            return;
        }
        obtained = false;
        obtainedPlayer = null;
        OnLose(player);
        StartCoroutine(IWait(loseDuration, next));
    }

    IEnumerator IObtain(float duration, Action next) {
        yield return new WaitForSeconds(duration);
        obtained = true;
        next();
    }

    IEnumerator IWait(float duration, Action next) {
        yield return new WaitForSeconds(duration);
        next();
    }

    public abstract void OnObtain(Player player);

    public abstract void OnLose(Player player);
}
