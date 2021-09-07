using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  [Header("Player UI controls")]
  public Button EndTurnButton;
  public TMP_Text ButtonText;

  [HideInInspector]
  public PlayerCharacter player;
  [HideInInspector]
  public List<EnemyCharacter> enemyList = new List<EnemyCharacter>();

  private int currentTurnIndex = 0;
  private List<Turn> turns = new List<Turn>();

  private void Start() {
    player = GridManager.Instance.Player;
    enemyList = GridManager.Instance.EnemyList;

    EndTurnButton.onClick.AddListener(() => player.EndTurn());

    SetupTurns();
  }

  private void SetupTurns() {
    turns.Add(GetComponentInChildren<ShowFurniturePreviewTurn>());

    turns.AddRange(enemyList.Select(enemy => enemy.Turn));

    turns.Add(player.Turn);

    turns.Add(GetComponentInChildren<SpawnFurnitureTurn>());
  }

  private void Update() {
    Turn currentTurn = turns[currentTurnIndex];

    if (currentTurn.Execute(this) == TurnPhase.End) {
      currentTurn.Reset();

      currentTurnIndex = (currentTurnIndex + 1) % turns.Count;
    }
  }

  public void UpdateButton(string text, bool isEnabled = false) {
    ButtonText.text = text;

    EndTurnButton.interactable = isEnabled;
  }
}
