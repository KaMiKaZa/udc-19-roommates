using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  private void Awake() {
    playerActor = GetComponent<PlayerActor>();

    Turn = GetComponentInChildren<Turn>();
  }

  private void Update() {
    // TODO: player can select movement direction before the actual turn

    if (Turn.Phase != TurnPhase.Start) {
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
