using UnityEngine;

public class CharacterTurn : Turn {
  private Character character;

  private float positionBlend;

  public Vector3 TargetPosition { get; set; }
  public Vector2 MoveDirection { get; set; }

  private void Awake() {
    character = GetComponentInParent<Character>();
  }

  public override void Reset() {
    base.Reset();

    MoveDirection = Vector2.zero;

    positionBlend = 0f;
  }

  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        Vector2? move = character.GetMove();

        if (move.HasValue) {
          TargetPosition = character.transform.position + (Vector3)move.Value;

          Phase = TurnPhase.Running;
        }

        break;
      }
      case TurnPhase.Running: {
        if (positionBlend < 1f) {
          character.transform.position = Vector3.Lerp(transform.position, TargetPosition, positionBlend);

          positionBlend += Time.deltaTime;
        } else {
          // to avoid any kind of precision error
          transform.position = TargetPosition;

          Phase = TurnPhase.End;
        }

        break;
      }
    }

    return Phase;
  }
}
