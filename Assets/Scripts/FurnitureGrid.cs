using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Furniture Grid consists of three items:
/// 1) Empty parent containing floor tiles
/// 2) Empty parent containing GridEntry objects which represents furniture placement
/// 3) Empty parent containing level's borders which may be either a wall or a window
/// </summary>
public class FurnitureGrid : MonoBehaviour {
  [Tooltip("Border collection")]
  public Transform Borders;

  [Header("Grid config"), Tooltip("GridEntry collection")]
  public Transform Grid;
  [Tooltip("Width of GridEntry collection")]
  public int GridWidth;
  [Tooltip("Height of GridEntry collection")]
  public int GridHeight;

  [Header("Game config")]
  public int EnemyCount;
}
