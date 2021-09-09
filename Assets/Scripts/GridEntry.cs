using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class GridEntry : MonoBehaviour {
  private CompositeCollider2D compositeCollider;
  public CompositeCollider2D CompositeCollider => compositeCollider;

  private void Awake() {
    compositeCollider = GetComponent<CompositeCollider2D>();
  }

  public void ActivatePreview() {
    gameObject.SetActive(true);
  }

  public void ActivateSpawn() {
    // return alpha back to 1f
    // it was 0.5f for a preview
    var renderers = GetComponentsInChildren<SpriteRenderer>();

    foreach (var renderer in renderers) {
      var color = renderer.color;
      color.a = 1f;
      renderer.color = color;
    }

    // increase size to visualize spawn effect
    // it will be reduced back to Vector3.one during the SpawnFurntitureTurn running phase
    transform.localScale = Vector3.one * 7.5f;
  }
}
