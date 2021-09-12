using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class GridEntry : MonoBehaviour {
  private CompositeCollider2D compositeCollider;
  public CompositeCollider2D CompositeCollider => compositeCollider;

  [SerializeField, Tooltip("Group of sprites representing the space occupied by this furniture")]
  private GameObject previewGroup;

  [SerializeField, Tooltip("Group of sprites representing this furniture")]
  private GameObject furnitureGroup;

  private void Awake() {
    compositeCollider = GetComponent<CompositeCollider2D>();
  }

  public void ActivatePreview() {
    gameObject.SetActive(true);

    previewGroup.SetActive(true);

    furnitureGroup.SetActive(false);
  }

  public void ActivateSpawn(System.Action onSpawnEndedCallback) {
    foreach (var sprite in previewGroup.GetComponentsInChildren<SpriteRenderer>()) {
      sprite.enabled = false;
    }

    furnitureGroup.SetActive(true);

    var initialScale = furnitureGroup.transform.localScale;

    Utils.RescaleOverTime(this, furnitureGroup.transform, initialScale * 8f, initialScale, onSpawnEndedCallback);
  }
}
