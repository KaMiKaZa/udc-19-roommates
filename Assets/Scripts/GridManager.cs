using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour {
  public enum OccupyKind {
    None,
    Character,
    Furniture,
    Wall,
    Window,
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

  private Dictionary<Vector2, OccupyKind> gridOccupation = new Dictionary<Vector2, OccupyKind>();

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

    foreach (var border in selectedGrid.GetComponentsInChildren<Border>()) {
      gridOccupation.Add(border.transform.position, (border is Wall) ? OccupyKind.Wall : OccupyKind.Window);
    }

    for (int i = 0; i < selectedGrid.EnemyCount; i++) {
      int index = Random.Range(0, enemyPrefabs.Length);

      Vector2 spawnPosition;

      do {
        spawnPosition = new Vector2(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)));
      } while (GetCellStatus(spawnPosition) != OccupyKind.None);

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

  public GridEntry ActivateSpawn(System.Action onSpawnEndedCallback) {
    currentFurniture.ActivateSpawn(onSpawnEndedCallback);

    spawnedFurnitureList.Add(currentFurniture);

    return currentFurniture;
  }

  public OccupyKind GetCellStatus(Vector2 position) {
    Vector3 worldPosition = new Vector3(position.x, position.y, 0f);

    if (gridOccupation.ContainsKey(position)) {
      return gridOccupation[position];
    }

    if (spawnedFurnitureList.Any(entry => entry.CompositeCollider.OverlapPoint(position))) {
      return OccupyKind.Furniture;
    }

    if (characterList.Any(character => character.transform.position == worldPosition)) {
      return OccupyKind.Character;
    }

    return OccupyKind.None;
  }

  public bool IsInputValid(Vector2 positionFrom, Vector2 inputDirection) {
    var targetOccupyKind = GetCellStatus(positionFrom + inputDirection);

    if (targetOccupyKind == OccupyKind.Character) {
      var pushPosOccupyKind = GetCellStatus(positionFrom + inputDirection * 2);

      // character can push another character only into an empty cell or a window
      return pushPosOccupyKind == OccupyKind.None || pushPosOccupyKind == OccupyKind.Window;
    }

    // character can freely move into an empty cell or a window
    return targetOccupyKind == OccupyKind.None || targetOccupyKind == OccupyKind.Window;
  }

  public Character GetCharacterAtCell(Vector2 position) {
    return characterList
      .Where(character => character.transform.position == (Vector3)position)
      .FirstOrDefault();
  }

  private IEnumerator MoveCoroutine(Transform characterTransform, Vector2 direction, System.Action callback = null) {
    float positionBlend = 0f;

    Vector2 targetPosition = (Vector2)characterTransform.position + direction;

    while (positionBlend < 1f) {
      characterTransform.position = Vector2.Lerp(characterTransform.position, targetPosition, positionBlend);

      positionBlend += Time.deltaTime;
    
      yield return null;
    }

    callback?.Invoke();
  }

  public void MoveCharacter(Character character, Vector2 direction, System.Action onMoveEndCallback) {
    Vector2 targetPosition = (Vector2)character.transform.position + direction;

    var occupyKind = GetCellStatus(targetPosition);

    switch (occupyKind) {
      case OccupyKind.None: {
        StartCoroutine(MoveCoroutine(character.transform, direction, onMoveEndCallback));

        break;
      }
      case OccupyKind.Character: {
        var characterToPush = GetCharacterAtCell(targetPosition);

        MoveCharacter(characterToPush, direction, onMoveEndCallback);
        //StartCoroutine(MoveCoroutine(character.transform, direction, onMoveEndCallback));

        break;
      }
      case OccupyKind.Window: {
        onMoveEndCallback += () => character.OnWindowDeath();

        StartCoroutine(MoveCoroutine(character.transform, direction * 2, onMoveEndCallback));

        break;
      }
    }
  }

  public void CheckForDeadCharacters() {
    // TODO: stop the game if player is dead

    var deadCharacters = CharacterList.Where(character => GetCellStatus(character.transform.position) == OccupyKind.Furniture);

    foreach (var character in deadCharacters) {
      character.IsAlive = false;
    }
  }
}
