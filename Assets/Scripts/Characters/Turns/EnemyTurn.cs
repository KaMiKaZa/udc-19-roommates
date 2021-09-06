using UnityEngine;

public class EnemyTurn : CharacterTurn {
  private void Awake() {
    character = GetComponentInParent<EnemyCharacter>(); 
  }

  public override TurnPhase Execute(TurnManager manager) {
    if (Phase == TurnPhase.Start) {
      manager.UpdateButton("Enemy's turn");
      
      Phase = TurnPhase.Running;

      Utils.DelayCall(this, () => {
        Phase = TurnPhase.End;
      }, 0.5f);
    }

    return Phase;
  }
}
