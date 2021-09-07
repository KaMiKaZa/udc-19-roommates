using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour {
  public enum OccupyKind {
    Character,
    Wall,
  }

  public static GridManager Instance { get; private set; }

  [SerializeField, Tooltip("Actual game level will be loaded from one of these grids")]
  private FurnitureGrid[] FurnitureGridPrefabs;

  [Header("Character Prefabs")]
  public PlayerCharacter playerPrefab;
  public EnemyCharacter[] enemyPrefabs;

  // grid
  private FurnitureGrid selectedGrid;
  private Queue<GridEntry> furnitureQueue = new Queue<GridEntry>();

  private GridEntry currentFurniture;
  public GridEntry CurrentFurniture => currentFurniture;

  // characters
  private PlayerCharacter player;
  private List<EnemyCharacter> enemyList = new List<EnemyCharacter>();

  public PlayerCharacter Player => player;
  public List<EnemyCharacter> EnemyList => enemyList;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }

    selectedGrid = Instantiate(FurnitureGridPrefabs[Random.Range(0, FurnitureGridPrefabs.Length)], transform);
    var orderedGridEntries = selectedGrid.Grid
      .GetComponentsInChildren<GridEntry>()
      .OrderByDescending(entry => entry.transform.childCount);

    foreach (var entry in orderedGridEntries) {
      entry.gameObject.SetActive(false);

      furnitureQueue.Enqueue(entry);
    }

    player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

    for (int i = 0; i < selectedGrid.EnemyCount; i++) {
      int index = Random.Range(0, enemyPrefabs.Length);

      Vector2 spawnPosition;

      do {
        spawnPosition = new Vector2(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)));
      } while (IsOccupied(spawnPosition));

      enemyList.Add(Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity));
    }
  }

  // TODO: return enum instead of bool
  public bool IsOccupied(Vector2 position) {
    Vector3 worldPosition = new Vector3(position.x, position.y, 0f);

    if (Mathf.Abs(position.x) > selectedGrid.GridWidth / 2 || Mathf.Abs(position.y) > selectedGrid.GridHeight / 2) {
      return true;
    }

    if (player.transform.position == worldPosition || enemyList.Any(enemy => enemy.transform.position == worldPosition)) {
      return true;
    }

    return false;
  }

  public bool CanMove(Vector2 positionFrom, Vector2 moveDirection) {
    return !IsOccupied(positionFrom + moveDirection);
  }

  public void EnableFurniturePreview() {
    if (furnitureQueue.Count > 0) {
      currentFurniture = furnitureQueue.Dequeue();

      currentFurniture.gameObject.SetActive(true);
    }
  }

  public GridEntry SpawnCurrentFurniture() {
    // return alpha back to 1f
    // it was 0.5f for a preview
    var renderers = currentFurniture.GetComponentsInChildren<SpriteRenderer>();

    foreach (var renderer in renderers) {
      var color = renderer.color;
      color.a = 1f;
      renderer.color = color;
    }

    // increase size to visualize spawn effect
    currentFurniture.transform.localScale = Vector3.one * 7.5f;

    return currentFurniture;
  }
}
