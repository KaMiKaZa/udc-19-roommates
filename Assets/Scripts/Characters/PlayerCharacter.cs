using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PlayerCharacter : Character {
  private PlayerActor playerActor;

  public Transform InvalidMoveIcon;
  public AudioSource InvalidMoveSound;

  protected override void Awake() {
    base.Awake();

    playerActor = GetComponent<PlayerActor>();
  }

  public override Vector2? GetMove() {
    UIManager.Instance.TurnInfoText.color = new Color(0.8f, 1f, 0.2f);
    UIManager.Instance.TurnInfoText.text = "Your turn";

    if (playerActor.move != Vector2.zero && playerActor.move.magnitude <= 1f) {
      if (GridManager.Instance.IsInputValid(transform.position, playerActor.move)) {
        return playerActor.move;
      } else {
        if (InvalidMoveSound && !InvalidMoveSound.isPlaying) {
          InvalidMoveSound.Play();
        }

        if (InvalidMoveIcon) {
          InvalidMoveIcon.gameObject.SetActive(true);
          InvalidMoveIcon.localPosition = playerActor.move;

          Utils.DelayCall(this, () => InvalidMoveIcon.gameObject.SetActive(false), 0.5f);
        }
      }
    }

    if (playerActor.turnEnd) {
      // TODO: may skip more than one turn!
      return Vector2.zero;
    }

    return null;
  }

  public override void OnDeath() {
    base.OnDeath();

    TurnManager.Instance.EndTheGame(false);
  }
}
