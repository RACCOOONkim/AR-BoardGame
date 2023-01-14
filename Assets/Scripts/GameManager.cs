using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager main;
    [Header("Preset Fields")]
    public Camera cam;

    public Dice dice;
    public int lastRoll = 0;

    public Player[] players;
    public int currentPlayer = 0;

    public BaseBoardSpace startSpace;
    public Quaternion startRotation;
    public GameObject targetPrefab, lightPiecePrefab, darkPiecePrefab;

    public bool goAgainPlayerFlag = false;

    [Header("Settings")]
    public int maxPieces = 5;
    public int pieceObjective = 3;

    [Header("Boards")]
    public BaseBoardSpace[] mars;
    public BaseBoardSpace[] uranus;

    [Header("Debug")]
    public GameState state = GameState.None;
    public int round = 0;

    private List<Item> tmpSeq = new List<Item>();

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
        PlaceItems();
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
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit rinfo, 50f, 1 << 12)) {
                        rinfo.transform.GetComponent<MoveTarget>().Clicked();
                    }
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
            p.Init();
        }
    }

    public void PlaceItems() {
        tmpSeq.Clear();
        for (int i = 0; i < maxPieces; i++) {
            tmpSeq.Add(Instantiate(lightPiecePrefab).GetComponent<Item>());
        }
        BoardUtils.PlaceItemsRandom(mars, tmpSeq, b => b.item == null && b != startSpace);

        tmpSeq.Clear();
        for (int i = 0; i < maxPieces; i++) {
            tmpSeq.Add(Instantiate(darkPiecePrefab).GetComponent<Item>());
        }
        BoardUtils.PlaceItemsRandom(uranus, tmpSeq, b => b.item == null && b != startSpace);
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

        CurrentPlayer().MoveConsumer(lastRoll, (b, h) => {
            MoveTarget mt = Instantiate(targetPrefab, b.transform).GetComponent<MoveTarget>();
            mt.Set(b, h);
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

    public void AdvancePlayer(Player player, BaseBoardSpace space, bool triggerEffects, List<BaseBoardSpace> history = null) {
        if(history is null) {
            player.FlyTo(space, () => {
                if (triggerEffects) {
                    space.StepOn(player, () => {
                        EndTurn();
                    });
                }
                else EndTurn();
            });
        }
        else {
            player.WalkTo(history, () => {
                if (triggerEffects) {
                    space.StepOn(player, () => {
                        EndTurn();
                    });
                }
                else EndTurn();
            });
        }
    }

    public void EndTurn() {
        if (state != GameState.EndTurn) {
            Debug.LogError("Wrong state error!");
            return;
        }

        if (CheckWinner()) {
            Debug.Log(CurrentPlayer().name + " wins!");
        }
        else {
            if (goAgainPlayerFlag) {
                goAgainPlayerFlag = false;
            }
            else {
                currentPlayer++;
                if (currentPlayer >= players.Length) {
                    currentPlayer = 0;
                    round++;
                }
            }
            state = GameState.WaitForTurn;
        }
    }

    private bool CheckWinner() {
        Player p = CurrentPlayer();
        return p.lightPiece >= pieceObjective && p.darkPiece >= pieceObjective;
    }
}
