using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoardSpace : MonoBehaviour {
    public abstract void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace, List<BaseBoardSpace>> cons, List<BaseBoardSpace> history);

    public void StepOn(Player player, Action next) {
        //todo
        next();
    }
}
