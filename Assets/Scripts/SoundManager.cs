using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
  private static float musicVolume = 1f;
  private static float sfxVolume = 1f;

  public AudioMixer Mixer;

  public Slider MusicSlider;
  public Slider SfxSlider;

  private void Start() {
    MusicSlider.value = musicVolume;
    SfxSlider.value = sfxVolume;
  }

  public void ChangeVolume(string parameter, float sliderVolume) {
    Mixer.SetFloat(parameter, Mathf.Log10(sliderVolume) * 20f);
  }

  public void ChangeMusicVolume(float newValue) {
    ChangeVolume("Music", newValue);

    musicVolume = newValue;
  }

  public void ChangeSfxVolume(float newValue) {
    ChangeVolume("Sfx", newValue);

    sfxVolume = newValue;
  }
}
