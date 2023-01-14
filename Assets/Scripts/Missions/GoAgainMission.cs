using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/GoAgain", order = 4)]
public class GoAgainMission : Mission {
    public GameObject useFx;
    public override void Go(Player player, BaseBoardSpace space, Action next) {
        if(useFx is not null) Instantiate(useFx, player.transform.position, player.transform.rotation);
        GameManager.main.goAgainPlayerFlag = true;
        next();
    }
}
