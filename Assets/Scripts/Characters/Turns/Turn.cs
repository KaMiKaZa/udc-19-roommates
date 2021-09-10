using System;
using UnityEngine;

public class Turn : MonoBehaviour {
  public virtual bool CanExecute => true;

  public TurnPhase Phase { get; set; }

  public virtual void Reset() {
    Phase = TurnPhase.Wait;
  }

  public virtual TurnPhase Execute(TurnManager manager) {
    return TurnPhase.End;
  }
}
