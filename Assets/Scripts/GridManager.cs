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
  private List<GridEntry> spawnedFurnitureList = new List<GridEntry>();

  private GridEntry currentFurniture;
  public GridEntry CurrentFurniture => currentFurniture;

  // characters
  private PlayerCharacter player;
  private List<Character> characterList = new List<Character>();
  public List<Character> CharacterList => characterList;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }

    selectedGrid = Instantiate(FurnitureGridPrefabs[Random.Range(0, FurnitureGridPrefabs.Length)], transform);

    foreach (var entry in selectedGrid.GetComponentsInChildren<GridEntry>()) {
      entry.gameObject.SetActive(false);

      furnitureQueue.Enqueue(entry);
    }

    for (int i = 0; i < selectedGrid.EnemyCount; i++) {
      int index = Random.Range(0, enemyPrefabs.Length);

      Vector2 spawnPosition;

      do {
        spawnPosition = new Vector2(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)));
      } while (IsOccupied(spawnPosition));

      characterList.Add(Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity));
    }

    player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    characterList.Add(player);
  }

  public void ActivatePreview() {
    if (furnitureQueue.Count > 0) {
      currentFurniture = furnitureQueue.Dequeue();

      currentFurniture.ActivatePreview();
    }
  }

  public GridEntry ActivateSpawn() {
    currentFurniture.ActivateSpawn();

    spawnedFurnitureList.Add(currentFurniture);

    return currentFurniture;
  }

  // TODO: return enum instead of bool
  public bool IsOccupied(Vector2 position) {
    Vector3 worldPosition = new Vector3(position.x, position.y, 0f);

    // TODO: check for walls and windows
    if (Mathf.Abs(position.x) > selectedGrid.GridWidth / 2 || Mathf.Abs(position.y) > selectedGrid.GridHeight / 2) {
      return true;
    }

    if (characterList.Any(character => character.transform.position == worldPosition)) {
      return true;
    }

    if (spawnedFurnitureList.Any(entry => entry.CompositeCollider.OverlapPoint(position))) {
      return true;
    }

    return false;
  }

  public bool CanMove(Vector2 positionFrom, Vector2 moveDirection) {
    return !IsOccupied(positionFrom + moveDirection);
  }
}
