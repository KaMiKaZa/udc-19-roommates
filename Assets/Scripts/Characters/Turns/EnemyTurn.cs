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

        // TODO: try to move away from next furniture's position
        Vector2 verticalOrHorizontal = (Random.Range(0f, 1f) > 0.5f) ? Vector2.up : Vector2.right;
        float positiveOrNegative = (Random.Range(0f, 1f) > 0.5f) ? 1f : -1f;

        MoveDirection = verticalOrHorizontal * positiveOrNegative;

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
