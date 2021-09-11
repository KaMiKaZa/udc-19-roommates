using UnityEngine;

public class CharacterTurn : Turn {
  private Character character;

  public override bool CanExecute => character.IsAlive;

  private void Awake() {
    character = GetComponentInParent<Character>();
  }

  public override void Reset() {
    base.Reset();
  }

  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        Vector2? move = character.GetMove();

        if (move.HasValue) {
          if (move.Value == Vector2.zero) {
            Phase = TurnPhase.End;
          } else {
            GridManager.Instance.MoveCharacter(character, move.Value, () => Phase = TurnPhase.End);

            Phase = TurnPhase.Running;
          }
        }

        break;
      }
    }

    return Phase;
  }
}
