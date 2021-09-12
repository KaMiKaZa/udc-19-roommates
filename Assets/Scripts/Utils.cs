using System;
using System.Collections;
using UnityEngine;

public class Utils {
  private static IEnumerator DelayCoroutine(Action action, float delay) {
    yield return new WaitForSeconds(delay);

    action?.Invoke();
  }
  
  public static void DelayCall(MonoBehaviour holder, Action action, float delayInSeconds = 0f) {
    if (delayInSeconds < 0f) {
      return;
    }

    holder.StartCoroutine(DelayCoroutine(action, delayInSeconds));
  }

  private static IEnumerator RescaleCoroutine(Transform transform, Vector3 end, Action action, float duration) {
    float scaleBlend = 0f;

    while (scaleBlend < 1f) {
      transform.localScale = Vector3.Lerp(transform.localScale, end, scaleBlend);

      scaleBlend += Time.deltaTime / duration;

      yield return null;
    }

    transform.localScale = end;

    action?.Invoke();
  }

  public static void RescaleOverTime(MonoBehaviour holder, Transform transform, Vector3 start, Vector3 end, Action action = null, float duration = 1f) {
    if (duration < 0f) {
      return;
    }

    transform.localScale = start;

    holder.StartCoroutine(RescaleCoroutine(transform, end, action, duration));
  }
}
