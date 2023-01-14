using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoardSpace : MonoBehaviour {
    public Item item;
    public Mission mission;

    public abstract void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons, List<BaseBoardSpace> history);

    public void StepOn(Player player, Action next) {
        if(item is null) {
            DoMission(player, next);
        }
        else {
            item.Get(player, () => {
                DoMission(player, next);
            });
        }
    }

    private void DoMission(Player player, Action next) {
        if (mission is null) {
            next();
        }
        else {
            mission.Go(player, this, next);
        }
    }
}
