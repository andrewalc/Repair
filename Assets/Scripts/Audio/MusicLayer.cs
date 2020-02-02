using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLayer : MonoBehaviour
{
	public AudioSource[] audioSources = new AudioSource[2];
	private int playingAudioIndex = 0;
	public AudioSource AudioSource => audioSources[playingAudioIndex];

	public void SetTrack(AudioClip clip)
	{
		audioSources[0].clip = clip;
		audioSources[1].clip = clip;
	}

	public void FadeIn()
	{
		StartCoroutine(FadeInRoutine());
	}

	public IEnumerator FadeInRoutine()
	{
		var audioSource = audioSources[playingAudioIndex];
		while(audioSource.volume < .9f)
		{
			audioSource.volume = Mathf.Lerp(audioSource.volume, 1f, .5f * Time.deltaTime);
			yield return null;
		}
		audioSource.volume = 1f;
	}

	public void FadeOut()
	{
		StartCoroutine(FadeOutRoutine());
	}

	public IEnumerator FadeOutRoutine()
	{
		var audioSource = audioSources[playingAudioIndex];
		while (audioSource.volume > .1f)
		{
			audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, .5f * Time.deltaTime);
			yield return null;
		}
		audioSource.volume = 0f;
	}

	public void SwitchSource()
	{
		playingAudioIndex = (playingAudioIndex + 1) % 2;
	}
}
