using UnityEngine;

public class ShowFurniturePreviewTurn : Turn {
  public override TurnPhase Execute(TurnManager manager) {
    if (Phase == TurnPhase.Start) {
      UIManager.Instance.TurnInfoText.text = "Furniture Incoming";

      Phase = TurnPhase.Running;

      Utils.DelayCall(this, () => {
        GridManager.Instance.ActivatePreview();

        Phase = TurnPhase.End;
      }, 0.5f);
    }

    return Phase;
  }
}
