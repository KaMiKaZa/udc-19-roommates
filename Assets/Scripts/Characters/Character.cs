using System;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour {
  public bool IsAlive { get; set; }

  public CharacterTurn[] Turns { get; protected set; }
  
  public bool IsMyTurn => Turns.Any(turn => turn.Phase != TurnPhase.Wait);

  protected virtual void Awake() {
    IsAlive = true;

    Turns = GetComponentsInChildren<CharacterTurn>();
  }

  public virtual Vector2? GetMove() {
    return Vector2.zero;
  }

  public virtual void OnDeath() {
    IsAlive = false;
  }

  public virtual void OnWindowDeath() {
    IsAlive = false;

    Utils.RescaleOverTime(this, transform, transform.localScale, Vector3.one * 0.01f, () => gameObject.SetActive(false));
  }
}
