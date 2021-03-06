using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
  public Sprite[] EnemySprites;

  protected override void Awake() {
    base.Awake();

    GetComponentInChildren<SpriteRenderer>().sprite = EnemySprites[Random.Range(0, EnemySprites.Length)];
  }

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
    } while (attemps.Count > 0 && !GridManager.Instance.IsInputValid(transform.position, selection));

    // skip the turn if enemy have no way to go
    if (!GridManager.Instance.IsInputValid(transform.position, selection)) {
      selection = Vector2.zero;
    }

    return selection;
  }
}
