using System;
using UnityEngine;

public abstract class BaseBoardSpace : MonoBehaviour {
    public abstract void Next(BaseBoardSpace previous, int n, Action<BaseBoardSpace> cons);
}
