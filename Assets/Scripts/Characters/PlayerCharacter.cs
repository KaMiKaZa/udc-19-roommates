using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  private Vector2? chosenMove = null;

  protected override void Awake() {
    base.Awake();

    playerActor = GetComponent<PlayerActor>();
  }

  private void Update() {
    // TODO: player can select movement direction before the actual turn

    if (!IsMyTurn) {
      chosenMove = null;

      return;
    }

    if (playerActor.move != Vector2.zero && playerActor.move.magnitude <= 1f) {
      if (GridManager.Instance.IsInputValid(transform.position, playerActor.move)) {
        chosenMove = playerActor.move;
      }
    }
  }

  public override Vector2? GetMove() {
    UIManager.Instance.TurnInfoText.text = "Your turn";

    return chosenMove;
  }
}
