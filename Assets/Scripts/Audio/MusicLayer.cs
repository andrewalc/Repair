using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLayer : MonoBehaviour
{
	public AudioSource[] audioSources = new AudioSource[2];
	private int playingAudioIndex = 0;
	public AudioSource AudioSource => audioSources[playingAudioIndex];

	private Coroutine fadeInRoutine;
	private Coroutine fadeOutRoutine;

	public void SetTrack(AudioClip clip)
	{
		audioSources[0].clip = clip;
		audioSources[1].clip = clip;
	}

	public void FadeIn()
	{
		if (fadeInRoutine != null)
		{
			return;
		}
		fadeInRoutine = StartCoroutine(FadeInRoutine());
	}

	public IEnumerator FadeInRoutine()
	{
		var audioSource = audioSources[playingAudioIndex];
		while(audioSource.volume < .99f)
		{
			if (IsFadingOut())
			{
				// We've started a fade out routine - stop this one.
				break;
			}
			
			audioSource.volume = Mathf.Lerp(audioSource.volume, 1f, Time.deltaTime);
			yield return null;
		}
		audioSource.volume = 1f;
		fadeInRoutine = null;
	}

	public void FadeOut()
	{
		if (fadeOutRoutine != null)
		{
			return;
		}
		fadeOutRoutine = StartCoroutine(FadeOutRoutine());
	}

	public IEnumerator FadeOutRoutine()
	{
		var audioSource = audioSources[playingAudioIndex];
		while (audioSource.volume > .01f)
		{
			if (IsFadingIn())
			{
				// We've started a fade in routine - stop this one.
				break;
			}
			
			audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime);
			yield return null;
		}
		audioSource.volume = 0f;
		fadeOutRoutine = null;
	}

	public bool IsFadingIn()
	{
		return fadeInRoutine != null;
	}
	
	public bool IsFadingOut()
	{	
		return fadeOutRoutine != null;
	}

	public bool IsFadedIn()
	{
		return AudioSource.volume > 0.9f && !IsFadingIn();
	}

	public bool IsFadedOut()
	{
		return AudioSource.volume < 0.1f && !IsFadingOut();
	}

	public void SwitchSource()
	{
		playingAudioIndex = (playingAudioIndex + 1) % 2;
	}

	public void SetVolume(float soundVolume)
	{
		AudioSource.volume = soundVolume;
	}
}
