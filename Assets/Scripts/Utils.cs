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
}
