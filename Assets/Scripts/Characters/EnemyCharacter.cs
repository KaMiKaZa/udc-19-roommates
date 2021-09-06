using UnityEngine;

public class EnemyCharacter : Character {
  private void Awake() {
    Turn = GetComponentInChildren<Turn>();
  }
}
