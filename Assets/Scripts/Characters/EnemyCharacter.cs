using UnityEngine;

public class EnemyCharacter : MonoBehaviour, ICharacter {
  public bool IsAlive => true;

  private Turn turn;
  public Turn Turn => turn;

  private void Awake() {
    turn = GetComponentInChildren<Turn>();
  }
}
