using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
  [Header("Prefabs")]
  public PlayerCharacter playerPrefab;
  public List<EnemyCharacter> enemyPrefabs = new List<EnemyCharacter>();

  [Header("Game settings")]
  public int EnemyCount = 2;

  [Header("Player UI controls")]
  public Button EndTurnButton;
  public TMP_Text ButtonText;

  [HideInInspector]
  public PlayerCharacter player;
  [HideInInspector]
  public List<EnemyCharacter> enemies = new List<EnemyCharacter>();

  private int currentTurnIndex = 0;
  private List<Turn> turns = new List<Turn>();


  private void Start() {
    player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

    EndTurnButton.onClick.AddListener(() => player.EndTurn());

    enemies.Clear();

    for (int i = 0; i < EnemyCount; i++) {
      int index = Random.Range(0, enemyPrefabs.Count);
      // TODO: test if space is already occupied
      Vector3 position = new Vector3(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)), 0f);

      enemies.Add(Instantiate(enemyPrefabs[index], position, Quaternion.identity));
    }

    SetupTurns();
  }

  private void SetupTurns() {
    turns.Add(GetComponentInChildren<ShowFurniturePreviewTurn>());

    turns.AddRange(enemies.Select(enemy => enemy.Turn));

    turns.Add(player.Turn);
  }

  private void Update() {
    Turn currentTurn = turns[currentTurnIndex];

    if (currentTurn.Execute(this) == Turn.TurnPhase.End) {
      currentTurn.Reset();

      currentTurnIndex = (currentTurnIndex + 1) % turns.Count;
    }
  }

  public void UpdateButton(string text, bool isEnabled = false) {
    ButtonText.text = text;

    EndTurnButton.interactable = isEnabled;
  }
}
