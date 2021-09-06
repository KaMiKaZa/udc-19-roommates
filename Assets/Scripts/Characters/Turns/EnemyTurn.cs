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

        // TODO: remove diagonal moves
        // TODO: try to move away from next furniture's position
        MoveDirection = new Vector2(Mathf.Round(Random.Range(-1f, 1f)), Mathf.Round(Random.Range(-1f, 1f)));

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
