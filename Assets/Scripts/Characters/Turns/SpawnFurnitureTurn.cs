using System.Linq;
using UnityEngine;

public class SpawnFurnitureTurn : Turn {
  private GridEntry gridEntry;

  private float scaleBlend;

  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        UIManager.Instance.TurnInfoText.text = "Furniture Is Here";

        gridEntry = GridManager.Instance.ActivateSpawn();

        scaleBlend = 0f;

        Phase = TurnPhase.Running;

        break;
      }
      case TurnPhase.Running: {
        if (scaleBlend < 1f) {
          gridEntry.transform.localScale = Vector3.Lerp(gridEntry.transform.localScale, Vector3.one, scaleBlend);

          scaleBlend += Time.deltaTime;
        } else {
          GridManager.Instance.CheckForDeadCharacters();

          gridEntry.transform.localScale = Vector3.one;

          Phase = TurnPhase.End;
        }

        break;
      }
    }

    return Phase;
  }
}
