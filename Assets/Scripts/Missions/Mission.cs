using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Mission", menuName = "Missions/Mission", order = 1)]
public abstract class Mission : ScriptableObject {
    public abstract void Go(Player player, BaseBoardSpace space, Action next);
}
