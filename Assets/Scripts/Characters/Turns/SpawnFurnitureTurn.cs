using System.Linq;
using UnityEngine;

public class SpawnFurnitureTurn : Turn {
  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        UIManager.Instance.TurnInfoText.text = "Furniture Is Here";

        GridManager.Instance.ActivateSpawn(() => {
          GridManager.Instance.CheckForDeadCharacters();
          
          Phase = TurnPhase.End;
        });

        Phase = TurnPhase.Running;

        break;
      }
    }

    return Phase;
  }
}
