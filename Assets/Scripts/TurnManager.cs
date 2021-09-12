using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  public static TurnManager Instance { get; private set; }

  private bool isGameEnded = false;

  private int currentTurnIndex = 0;
  private List<Turn> turns = new List<Turn>();

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }

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
    if (isGameEnded) {
      return;
    }

    Turn currentTurn = turns[currentTurnIndex];

    if (currentTurn.CanExecute) {
      if (currentTurn.Phase == TurnPhase.Wait) {
        currentTurn.Phase = TurnPhase.Start;
      }

      if (currentTurn.Execute(this) == TurnPhase.End) {
        currentTurn.Reset();

        currentTurnIndex = (currentTurnIndex + 1) % turns.Count;
      }
    } else {
      currentTurnIndex = (currentTurnIndex + 1) % turns.Count;
    }
  }

  public int GetRemainingTurnCount(Character character) {
    return turns
      .Where(turn => turn is CharacterTurn)
      .Cast<CharacterTurn>()
      .Where((turn, index) => turn.Character == character && index >= currentTurnIndex)
      .Count();
  }

  public void EndTheGame(bool playerWon = true) {
    UIManager.Instance.EndGameText.text = (playerWon) ? "You're survived! Well done!" : ">>> YOU DIED <<<";

    UIManager.Instance.SidebarGroup.gameObject.SetActive(false);
    UIManager.Instance.EndGameScreenGroup.gameObject.SetActive(true);

    isGameEnded = true;
  }
}
