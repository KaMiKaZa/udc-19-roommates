using UnityEngine;

public class PlayerTurn : Turn {
  private PlayerCharacter player;

  private void Awake() {
    player = GetComponentInParent<PlayerCharacter>();
  }

  public override TurnPhase Execute(TurnManager manager) {
    if (Phase == TurnPhase.Start) {
      manager.UpdateButton("End your turn", true);

      Phase = TurnPhase.Running;
    }

    return Phase;
  }
}
