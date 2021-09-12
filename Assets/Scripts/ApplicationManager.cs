using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour {
  public void PlayAgain() {
    SceneManager.LoadScene(0);
  }

  public void Quit() {
    Application.Quit();
  }
}
