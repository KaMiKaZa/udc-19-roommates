using UnityEngine;

public class SpawnFurnitureTurn : Turn {
  [Tooltip("Time to spawn furniture in seconds")]
  public float spawnTime = 2f;

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
        if (gridEntry.transform.localScale != Vector3.one) {
          scaleBlend += Time.deltaTime;

          gridEntry.transform.localScale = Vector3.Lerp(gridEntry.transform.localScale, Vector3.one, scaleBlend / spawnTime);
        } else {
          // TODO: kill all affected characters

          Phase = TurnPhase.End;
        }

        break;
      }
    }

    return Phase;
  }
}
