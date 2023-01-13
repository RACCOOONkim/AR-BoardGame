using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPiece : Item {
    public int amount = 1;
    public bool isLight = true;
    public GameObject lightFx;

    public override void OnObtain(Player player) {
        if (isLight) player.lightPiece += amount;
        else player.darkPiece += amount;

        if (lightFx is not null) {
            Instantiate(lightFx, player.transform.position, Quaternion.identity);
        }
    }

    public override void OnLose(Player player) {
        if (isLight) player.lightPiece -= amount;
        else player.darkPiece -= amount;

        //place in random board pos
        BoardUtils.PlaceItemRandom(isLight ? GameManager.main.mars : GameManager.main.uranus, this, b => b.item == null);
    }
}
