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

  public AudioSource WindowBreak;
  public ParticleSystem WindowParticles;

  // grid
  private FurnitureGrid selectedGrid;
  public Queue<GridEntry> FurnitureQueue = new Queue<GridEntry>();
  private List<GridEntry> spawnedFurnitureList = new List<GridEntry>();

  private GridEntry currentFurniture;
  public GridEntry CurrentFurniture => currentFurniture;

  private Dictionary<Vector2, OccupyKind> gridOccupation = new Dictionary<Vector2, OccupyKind>();

  // characters
  private List<Character> characterList = new List<Character>();
  public List<Character> CharacterList => characterList;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }

    selectedGrid = Instantiate(FurnitureGridPrefabs[Random.Range(0, FurnitureGridPrefabs.Length)], transform);

    foreach (var entry in selectedGrid.GetComponentsInChildren<GridEntry>()) {
      entry.gameObject.SetActive(false);

      FurnitureQueue.Enqueue(entry);
    }

    foreach (var border in selectedGrid.GetComponentsInChildren<Border>()) {
      gridOccupation.Add(border.transform.position, (border is Wall) ? OccupyKind.Wall : OccupyKind.Window);
    }

    for (int i = 0; i < selectedGrid.EnemyCount + 1; i++) {
      int index = Random.Range(0, enemyPrefabs.Length);

      Vector2 spawnPosition;

      do {
        spawnPosition = new Vector2(Mathf.Round(Random.Range(-2f, 2f)), Mathf.Round(Random.Range(-2f, 2f)));
      } while (characterList.Any(character => (Vector2)character.transform.position == spawnPosition));

      Character character;

      if (i < selectedGrid.EnemyCount) {
        character = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
      } else {
        // it's the last iteration in the case of i == selectedGrid.EnemyCount
        character = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
      }

      characterList.Add(character);
    }

  }

  public void ActivatePreview() {
    if (FurnitureQueue.Count > 0) {
      currentFurniture = FurnitureQueue.Dequeue();

      currentFurniture.ActivatePreview();
    }
  }

  public GridEntry ActivateSpawn(System.Action onSpawnActivatedCallback, System.Action onSpawnEndedCallback) {
    currentFurniture.ActivateSpawn(onSpawnEndedCallback);

    spawnedFurnitureList.Add(currentFurniture);

    onSpawnActivatedCallback?.Invoke();

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

  private IEnumerator MoveCoroutine(Transform characterTransform, Vector2 direction, System.Action callback = null, float duration = 1f) {
    float positionBlend = 0f;

    Vector2 targetPosition = (Vector2)characterTransform.position + direction;

    while (positionBlend < 1f) {
      characterTransform.position = Vector2.Lerp(characterTransform.position, targetPosition, positionBlend);

      positionBlend += Time.deltaTime / duration;
    
      yield return null;
    }

    characterTransform.position = targetPosition;

    callback?.Invoke();
  }

  public void MoveCharacter(Character character, Vector2 direction, System.Action onMoveEndCallback) {
    Vector2 targetPosition = (Vector2)character.transform.position + direction;

    var occupyKind = GetCellStatus(targetPosition);

    switch (occupyKind) {
      case OccupyKind.None: {
        if (character.MoveSound) {
          character.MoveSound.Play();
        }

        StartCoroutine(MoveCoroutine(character.transform, direction, onMoveEndCallback, 0.5f));

        break;
      }
      case OccupyKind.Character: {
        var characterToPush = GetCharacterAtCell(targetPosition);

        MoveCharacter(characterToPush, direction, onMoveEndCallback);
        //StartCoroutine(MoveCoroutine(character.transform, direction, onMoveEndCallback));

        if (character.PushSound) {
          character.PushSound.Play();
        }

        break;
      }
      case OccupyKind.Window: {
        WindowParticles.transform.position = targetPosition;
        WindowParticles.gameObject.SetActive(true);
        WindowParticles.Play();

        WindowBreak.Play();

        onMoveEndCallback += () => {
          character.OnWindowDeath();

          CheckForDeadCharacters();
        };

        StartCoroutine(MoveCoroutine(character.transform, direction * 2, onMoveEndCallback, 0.5f));

        break;
      }
    }
  }

  public void CheckForDeadCharacters() {
    var deadByFurniture = CharacterList.Where(character => GetCellStatus(character.transform.position) == OccupyKind.Furniture);

    foreach (var character in deadByFurniture) {
      character.OnFurnitureDeath();
    }

    characterList = characterList.Where(character => character.IsAlive).ToList();

    UIManager.Instance.AliveCountText.text = "---";

    if (characterList.Count == 1) {
      // don't need to check if player is alive because player's death also calls EndTheGame()

      TurnManager.Instance.EndTheGame();
    }
  }
}
