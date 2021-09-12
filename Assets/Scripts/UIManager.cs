using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
  public static UIManager Instance { get; private set; }

  public TMP_Text TurnInfoText;
  public TMP_Text AliveCountText;

  public TMP_Text EndGameText;

  public RectTransform SidebarGroup;
  public RectTransform EndGameScreenGroup;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }

    SidebarGroup.gameObject.SetActive(true);
    EndGameScreenGroup.gameObject.SetActive(false);
  }
}
