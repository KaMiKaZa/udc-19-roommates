using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : MonoBehaviour, ICharacter {
  private PlayerActor playerActor;

  public bool IsAlive => true;

  private Turn turn;
  public Turn Turn => turn;

  private void Awake() {
    playerActor = GetComponent<PlayerActor>();

    turn = GetComponentInChildren<Turn>();
  }

  private void Update() {
    if (turn.Phase != Turn.TurnPhase.Running) {
      return;
    }

    if (playerActor.turnEnd) {
      EndTurn();
    }
  }

  public void EndTurn() {
    turn.Phase = Turn.TurnPhase.End;
  }
}
