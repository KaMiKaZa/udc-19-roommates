using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour {
  public enum GameState {
    Init,
    //EnemyTurn,
    //PlayerTurn,
    //SpawnFurniture,
    //Won,
    //Lose,
  }

  [Header("Prefabs")]
  public GameObject playerPrefab;
  public List<GameObject> enemyPrefabs = new List<GameObject>();

  [Header("Game settings")]
  public GameState CurrentState = GameState.Init;
  public int EnemyCount = 2;

  private GameObject player;
  private List<GameObject> enemies = new List<GameObject>();

  private void Start() {
    StartCoroutine(Init());
  }

  private IEnumerator Init() {
    player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

    enemies.Clear();

    yield return new WaitForSeconds(1f);

    for (int i = 0; i < EnemyCount; i++) {
      int index = Random.Range(0, enemyPrefabs.Count);
      Vector3 position = new Vector3(Mathf.Round(Random.Range(-3f, 3f)), Mathf.Round(Random.Range(-3f, 3f)), 0f);

      enemies.Add(Instantiate(enemyPrefabs[index], position, Quaternion.identity));

      yield return new WaitForSeconds(1f);
    }

    yield return null;
  }
}
