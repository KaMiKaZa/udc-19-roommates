using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  private void Awake() {
    playerActor = GetComponent<PlayerActor>();

    Turn = GetComponentInChildren<Turn>();
  }

  private void Update() {
    if (Turn.Phase != Turn.TurnPhase.Running) {
      return;
    }

    if (playerActor.turnEnd) {
      EndTurn();
    }
  }

  public void EndTurn() {
    Turn.Phase = Turn.TurnPhase.End;
  }
}
