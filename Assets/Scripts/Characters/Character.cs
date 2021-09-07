using System;
using UnityEngine;

public class Character : MonoBehaviour {
  public bool IsAlive { get; }

  public CharacterTurn Turn { get; protected set; }

  public virtual Vector2? GetMove() {
    return Vector2.zero;
  }
}
