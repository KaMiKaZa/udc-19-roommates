using System;
using UnityEngine;

public class Turn : MonoBehaviour {
  public enum TurnPhase {
    Start,
    Running,
    End,
  }

  public TurnPhase Phase { get; set; }

  public void Reset() {
    Phase = TurnPhase.Start;
  }

  public virtual TurnPhase Execute(TurnManager manager) {
    return TurnPhase.End;
  }
}
