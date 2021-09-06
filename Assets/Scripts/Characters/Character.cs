using UnityEngine;

public class Character : MonoBehaviour {
  public bool IsAlive { get; }

  public Turn Turn { get; protected set; }
}
