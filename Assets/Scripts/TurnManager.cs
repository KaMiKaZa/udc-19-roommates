using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  private int currentTurnIndex = 0;
  private List<Turn> turns = new List<Turn>();

  private void Start() {
    SetupTurns();
  }

  private void SetupTurns() {
    turns.Add(GetComponentInChildren<ShowFurniturePreviewTurn>());

    foreach (var characterTurns in GridManager.Instance.CharacterList.Select(enemy => enemy.Turns)) {
      turns.AddRange(characterTurns);
    }

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
