using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
	public List<MusicTrack> musicTracks;
	public MusicLayer[] musicLayers = new MusicLayer[5];
	private int currentTrackIndex = 0;
	private bool listenersSet = false;

	private Coroutine levelStartCoroutine;

	public static SoundManager Instance { get; private set; }

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void PlayNewTrack()
	{
		var r = new System.Random();
		SetTrack(r.Next(musicTracks.Count));
	}

	public void SetTrack(int trackIndex)
	{
		StartCoroutine(SetTrackRoutine(trackIndex));
	}

	private IEnumerator SetTrackRoutine(int trackIndex)
	{
		currentTrackIndex = trackIndex;
		for (int i = 0; i < 5; ++i)
		{
			musicLayers[i].SetTrack(musicTracks[trackIndex].layers[i]);
			musicLayers[i].SetVolume(0.0f);
			musicLayers[i].AudioSource.Play();
		}
		yield return null;
	}

	public void Update()
	{
		if (!listenersSet)
		{
			Tick.Instance.AddEventListener(SetLevel);
			Game.Instance.LevelEnded += OnLevelEnded;
			Game.Instance.LevelGenerated += OnLevelGenerated;
			listenersSet = true;
		}

		if (musicLayers[0].AudioSource.time >= musicTracks[currentTrackIndex].loopTime)
		{
			for(int i = 0; i < musicLayers.Length; ++i)
			{
				musicLayers[i].SwitchSource();
				musicLayers[i].AudioSource.Play();
			}
		}
	}

	private void OnLevelGenerated(Simulation sim)
	{
		if (null != levelStartCoroutine)
		{
			return;
		}
		
		levelStartCoroutine = StartCoroutine(StartLevelMusic());
	}

	private IEnumerator StartLevelMusic()
	{
		if (musicLayers[0].IsFadingOut())
		{
			yield return new WaitUntil(() => musicLayers[0].IsFadedOut());
		}

		PlayNewTrack();
		levelStartCoroutine = null;
	}
	
	private void OnLevelEnded(Simulation sim)
	{
		FadeAll();
	}

	public void SetLevel()
	{
		if (!Game.Instance.finishedGeneratingLevel)
		{
			return;
		}

		int level = Math.Min(musicLayers.Length, Mathf.FloorToInt(Game.Instance.Simulation.currentState.Sustainability / 20) + 1);
		for(int i = 0; i< level; ++i)
		{
			if(!musicLayers[i].IsFadingIn() && musicLayers[i].IsFadedOut())
			{
				musicLayers[i].FadeIn();
			}
		}
		for(int i = musicLayers.Length - 1; i >= level; --i)
		{
			if (!musicLayers[i].IsFadingOut() && musicLayers[i].IsFadedIn())
			{
				musicLayers[i].FadeOut();
			}
		}
	}

	public void FadeAll()
	{
		for (int i = 0; i < musicLayers.Length; ++i)
		{
			if (!musicLayers[i].IsFadingOut())
			{
				musicLayers[i].FadeOut();
			}
		}
	}
}
