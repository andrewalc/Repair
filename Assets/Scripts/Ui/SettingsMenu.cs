using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] private Slider masterVolume;
	[SerializeField] private Slider musicVolume;
	[SerializeField] private Slider sfxVolume;
	[SerializeField] private Slider brightness;

	private void Start()
	{
		masterVolume.onValueChanged.AddListener(SetMasterVolume);
		musicVolume.onValueChanged.AddListener(SetMusicVolume);
		sfxVolume.onValueChanged.AddListener(SetSFXVolume);
		brightness.onValueChanged.AddListener(SetBrightness);
	}

	private void OnEnable()
	{
		masterVolume.value = AudioListener.volume;
		musicVolume.value = SoundManager.Instance.MusicVolume;
		sfxVolume.value = SoundManager.Instance.SFXVolume;
		brightness.value = Mathf.Clamp(RenderSettings.ambientIntensity / 4, 0, 4);
	}

	private void OnDestroy()
	{
		masterVolume.onValueChanged.RemoveListener(SetMasterVolume);
		musicVolume.onValueChanged.RemoveListener(SetMusicVolume);
		sfxVolume.onValueChanged.RemoveListener(SetSFXVolume);
		brightness.onValueChanged.RemoveListener(SetBrightness);
	}


	private void SetMasterVolume(float volume)
	{
		AudioListener.volume = volume;
	}

	private void SetMusicVolume(float volume)
	{
		SoundManager.Instance.SetMusicVolume(volume);
	}

	private void SetSFXVolume(float volume)
	{
		SoundManager.Instance.SetSFXVolume(volume);
	}

	private void SetBrightness(float brightness)
	{
		brightness *= 4;
		RenderSettings.ambientIntensity = brightness;
	}

}
