using UnityEngine;

public class CharacterTurn : Turn {
  protected Character character;

  public Vector2 MoveDirection { get; set; }

  public override void Reset() {
    base.Reset();

    MoveDirection = Vector2.zero;
  }
}
