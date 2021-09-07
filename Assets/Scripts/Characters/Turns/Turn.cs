using System;
using UnityEngine;

public class Turn : MonoBehaviour {
  public TurnPhase Phase { get; set; }

  public virtual void Reset() {
    Phase = TurnPhase.Start;
  }

  public virtual TurnPhase Execute(TurnManager manager) {
    return TurnPhase.End;
  }
}
