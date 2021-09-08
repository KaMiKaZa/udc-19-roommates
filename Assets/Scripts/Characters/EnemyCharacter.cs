using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
  public override Vector2? GetMove() {
    UIManager.Instance.TurnInfoText.text = "Enemy's turn";

    List<Vector2> attemps = new List<Vector2>() {
      Vector2.up, Vector2.down, Vector2.right, Vector2.left,
    };
    Vector2 selection;

    do {
      // TODO: try to move away from next furniture's position

      selection = attemps[Random.Range(0, attemps.Count)];

      attemps.Remove(selection);
    } while (attemps.Count > 0 && !GridManager.Instance.CanMove(transform.position, selection));

    // skip the turn if enemy have no way to go
    if (!GridManager.Instance.CanMove(transform.position, selection)) {
      selection = Vector2.zero;
    }

    return selection;
  }
}
