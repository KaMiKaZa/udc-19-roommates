using UnityEngine;

public class ShowFurniturePreviewTurn : Turn {
  private void Awake() {
    Phase = TurnPhase.Start;
  }

  public override TurnPhase Execute(TurnManager manager) {
    if (Phase == TurnPhase.Start) {
      manager.UpdateButton("Next Furniture Incoming");

      Phase = TurnPhase.Running;

      Utils.DelayCall(this, () => {
        // TODO: do a proper retrieval from a pre-created furniture grid
        Vector3 position = new Vector3(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)), 0f);

        GridManager.Instance.EnableFurniturePreview();

        Phase = TurnPhase.End;
      }, 0.5f);
    }

    return Phase;
  }
}
