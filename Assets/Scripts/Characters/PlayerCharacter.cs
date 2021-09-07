using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  private Vector2? chosenMove = null;

  private void Awake() {
    playerActor = GetComponent<PlayerActor>();

    Turn = GetComponentInChildren<CharacterTurn>();
  }

  private void Update() {
    // TODO: player can select movement direction before the actual turn

    if (Turn.Phase != TurnPhase.Start) {
      chosenMove = null;

      return;
    }

    if (playerActor.move != Vector2.zero && playerActor.move.magnitude <= 1f) {
      if (GridManager.Instance.CanMove(transform.position, playerActor.move)) {
        chosenMove = playerActor.move;
      }
    }
  }

  public override Vector2? GetMove() {
    UIManager.Instance.TurnInfoText.text = "Your turn";

    return chosenMove;
  }
}
