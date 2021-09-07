using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : CharacterTurn {
  private Vector3 targetPosition;
  private float positionBlend;
  
  private void Awake() {
    character = GetComponentInParent<EnemyCharacter>(); 
  }

  public override void Reset() {
    base.Reset();

    positionBlend = 0f;
  }

  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        manager.UpdateButton("Enemy's turn");

        Phase = TurnPhase.Running;

        List<Vector2> attemps = new List<Vector2>() {
          Vector2.up, Vector2.down, Vector2.right, Vector2.left,
        };

        do {
          // TODO: try to move away from next furniture's position

          MoveDirection = attemps[Random.Range(0, attemps.Count)];

          attemps.Remove(MoveDirection);
        } while (attemps.Count > 0 && !GridManager.Instance.CanMove(transform.position, MoveDirection));

        // skip the turn if enemy have no way to go
        if (!GridManager.Instance.CanMove(transform.position, MoveDirection)) {
          MoveDirection = Vector2.zero;
        }

        targetPosition = transform.position + (Vector3)MoveDirection;
        
        break;
      }
      case TurnPhase.Running: {
        if (transform.position == targetPosition) {
          Phase = TurnPhase.End;
        } else {
          character.transform.position = Vector3.Lerp(transform.position, targetPosition, positionBlend);

          positionBlend += Time.deltaTime;
        }

        break;
      }
    }

    return Phase;
  }
}
