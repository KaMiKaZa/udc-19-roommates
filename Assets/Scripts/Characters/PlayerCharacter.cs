using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  protected override void Awake() {
    base.Awake();

    playerActor = GetComponent<PlayerActor>();
  }

  public override Vector2? GetMove() {
    UIManager.Instance.TurnInfoText.text = "Your turn";

    if (playerActor.move != Vector2.zero && playerActor.move.magnitude <= 1f) {
      if (GridManager.Instance.IsInputValid(transform.position, playerActor.move)) {
        return playerActor.move;
      }
    }

    if (playerActor.turnEnd) {
      // TODO: may skip more than one turn!
      return Vector2.zero;
    }

    return null;
  }
}
