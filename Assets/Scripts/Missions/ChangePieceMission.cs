using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/ChangePiece", order = 3)]
public class ChangePieceMission : Mission {
    public bool toLight = true;
    public GameObject changeFx;

    public override void Go(Player player, BaseBoardSpace space, Action next) {
        for (int i = 0; i < player.items.Count; i++) {
            if ((player.items[i] is LightPiece lp) && lp.isLight == !toLight) {
                if (changeFx is not null) Instantiate(changeFx, player.items[i].transform.position, Quaternion.identity);
                if (toLight) {
                    player.lightPiece += lp.amount;
                    player.darkPiece -= lp.amount;
                }
                else {
                    player.lightPiece -= lp.amount;
                    player.darkPiece += lp.amount;
                }

                LightPiece to = (toLight ? GameManager.main.lightPiecePrefab : GameManager.main.darkPiecePrefab).GetComponent<LightPiece>();
                lp.lightFx = to.lightFx;
                lp.obtainFx = to.obtainFx;
                lp.isLight = to.isLight;
                Transform formerModel = lp.transform.GetChild(0);
                Vector3 localPos = formerModel.localPosition;
                Quaternion localRot = formerModel.localRotation;
                Vector3 localScale = formerModel.localScale;
                Destroy(formerModel.gameObject);
                GameObject latterModel = Instantiate(to.transform.GetChild(0).gameObject, localPos, localRot, lp.transform);
                latterModel.transform.localScale = localScale;

                break;
            }
        }

        next();
    }
}
