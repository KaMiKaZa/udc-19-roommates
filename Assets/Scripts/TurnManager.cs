using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  // characters
  private PlayerCharacter player;
  private List<EnemyCharacter> enemyList = new List<EnemyCharacter>();

  // turns
  private int currentTurnIndex = 0;
  private List<Turn> turns = new List<Turn>();

  private void Start() {
    player = GridManager.Instance.Player;
    enemyList = GridManager.Instance.EnemyList;

    SetupTurns();
  }

  private void SetupTurns() {
    turns.Add(GetComponentInChildren<ShowFurniturePreviewTurn>());

    foreach (var enemyTurns in enemyList.Select(enemy => enemy.Turns)) {
      turns.AddRange(enemyTurns);
    }

    turns.AddRange(player.Turns);

    turns.Add(GetComponentInChildren<SpawnFurnitureTurn>());
  }

  private void Update() {
    Turn currentTurn = turns[currentTurnIndex];

    if (currentTurn.Phase == TurnPhase.Wait) {
      currentTurn.Phase = TurnPhase.Start;
    }

    if (currentTurn.Execute(this) == TurnPhase.End) {
      currentTurn.Reset();

      currentTurnIndex = (currentTurnIndex + 1) % turns.Count;
    }
  }
}
