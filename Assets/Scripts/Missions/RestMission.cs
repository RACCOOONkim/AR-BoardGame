using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Rest", order = 5)]
public class RestMission : Mission {
    public GameObject useFx;
    public override void Go(Player player, BaseBoardSpace space, Action next) {
        if (useFx is not null) Instantiate(useFx, player.transform.position, player.transform.rotation);
        int id = -1;
        for(int i=0; i< GameManager.main.players.Length; i++) {
            if (GameManager.main.players[i] == player) id = i;
        }
        if(id != -1) GameManager.main.restPlayerFlag[id] = true;
        next();
    }
}
