using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager main;

    public Dice dice;
    public int lastRoll = 0;

    public Player[] players;
    public int currentPlayer = 0;

    public BaseBoardSpace startSpace;
    public Quaternion startRotation;
    public GameObject targetPrefab;

    public GameState state = GameState.None;
    public int round = 0;

    public enum GameState {
        None,
        WaitForTurn,
        RollingDice,
        Choosing,
        EndTurn
    }

    private void Awake() {
        main = this;
    }

    private void Start() {
        Init();
    }

    private void Update() {
        //todo remove
        if (state == GameState.WaitForTurn && Input.GetMouseButtonDown(0)) {
            DiceRoll();
        }

        switch (state) {
            case GameState.RollingDice:
                if (!dice.IsRolling()) {
                    lastRoll = dice.GetResult();
                    StartPlayerChoice();
                }
                break;
            case GameState.Choosing:
                if (Input.GetMouseButtonDown(0)) {

                }
                break;
        }
    }

    public void Init() {
        state = GameState.WaitForTurn;
        currentPlayer = 0;
        round = 1;

        foreach (Player p in players) {
            p.SetSpace(startSpace);
            p.transform.rotation = startRotation;
        }
    }

    public Player CurrentPlayer() {
        return players[currentPlayer];
    }

    public void DiceRoll() {
        if (state != GameState.WaitForTurn) {
            Debug.LogError("Wrong state error!");
            return;
        }

        dice.Roll();
        state = GameState.RollingDice;
    }

    private void StartPlayerChoice() {
        state = GameState.Choosing;

        CurrentPlayer().MoveConsumer(lastRoll, b => {
            MoveTarget mt = Instantiate(targetPrefab, b.transform).GetComponent<MoveTarget>();
            mt.Set(b);
        });
        if (GameObject.FindGameObjectWithTag("MoveTarget") is null) {
            Debug.Log("Nowhere to go!");

            EndPlayerChoice();
            EndTurn();
        }
    }

    public void EndPlayerChoice() {
        if (state != GameState.Choosing) {
            Debug.LogError("Wrong state error!");
            return;
        }

        GameObject[] tgs = GameObject.FindGameObjectsWithTag("MoveTarget");
        foreach (GameObject t in tgs) {
            t.GetComponent<MoveTarget>().Hide();
        }

        state = GameState.EndTurn;
    }

    public void AdvancePlayer(Player player, BaseBoardSpace space, bool triggerEffects) {
        //todo move
    }

    public void EndTurn() {
        if (state != GameState.EndTurn) {
            Debug.LogError("Wrong state error!");
            return;
        }

        currentPlayer++;
        if (currentPlayer >= players.Length) {
            currentPlayer = 0;
            round++;
        }
        state = GameState.WaitForTurn;
    }
}
