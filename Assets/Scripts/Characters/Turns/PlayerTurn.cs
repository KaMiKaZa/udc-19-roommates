using UnityEngine;

public class PlayerTurn : CharacterTurn {
  private void Awake() {
    character = GetComponentInParent<PlayerCharacter>();
  }

  public override TurnPhase Execute(TurnManager manager) {
    if (Phase == TurnPhase.Start) {
      manager.UpdateButton("End your turn", true);

      Phase = TurnPhase.Running;
    }

    return Phase;
  }
}
