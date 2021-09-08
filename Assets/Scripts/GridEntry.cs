using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class GridEntry : MonoBehaviour {
  private CompositeCollider2D compositeCollider;
  public CompositeCollider2D CompositeCollider => compositeCollider;

  private void Awake() {
    compositeCollider = GetComponent<CompositeCollider2D>();
  }
}
