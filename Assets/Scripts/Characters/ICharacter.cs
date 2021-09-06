using UnityEngine;

public interface ICharacter {
  public bool IsAlive { get; }

  public Turn Turn { get; }
}
