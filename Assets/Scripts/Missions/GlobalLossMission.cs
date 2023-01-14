using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/GlobalLoss", order = 2)]
public class GlobalLossMission : Mission {
    public int amount = 99;
    public bool isLight = true;

    public GameObject loseFx;

    public override void Go(Player player, BaseBoardSpace space, Action next) {
        foreach (Player p in GameManager.main.players) {
            int c = amount;
            for (int i = 0; i < p.items.Count; i++) {
                if (c <= 0) break;
                if ((p.items[i] is LightPiece lp) && lp.isLight == isLight) {
                    c--;
                    if (loseFx is not null) Instantiate(loseFx, p.items[i].transform.position, Quaternion.identity);
                    p.items[i].Lose(p, () => { });
                    i--;//because its getting removed
                }
            }
        }
        next();
    }
}
