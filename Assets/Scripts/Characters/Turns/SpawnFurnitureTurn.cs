using UnityEngine;

public class SpawnFurnitureTurn : Turn {
  public AudioSource FurnitureFall;

  public override TurnPhase Execute(TurnManager manager) {
    switch (Phase) {
      case TurnPhase.Start: {
        // reset the color that player is assigned
        UIManager.Instance.TurnInfoText.color = new Color(1f, 1f, 1f);
        UIManager.Instance.TurnInfoText.text = "Furniture Is Here";

        GridManager.Instance.ActivateSpawn(
          () => {
            if (FurnitureFall) {
              FurnitureFall.Play();
            }

            GridManager.Instance.CheckForDeadCharacters();
          },
          () => Phase = TurnPhase.End
        );

        Phase = TurnPhase.Running;

        break;
      }
    }

    return Phase;
  }
}
