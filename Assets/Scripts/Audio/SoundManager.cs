using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
	public List<MusicTrack> musicTracks;
	public MusicLayer[] musicLayers = new MusicLayer[5];
	private int currentTrackIndex = 0;
	private bool listenersSet = false;

	public static SoundManager Instance { get; private set; }

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		//SetTrack(2);
	}

	public void PlayNewTrack()
	{
		var r = new System.Random();
		SetTrack(r.Next(musicTracks.Count - 1));
	}

	public void SetTrack(int trackIndex)
	{
		StartCoroutine(SetTrackRoutine(trackIndex));
	}


	private IEnumerator SetTrackRoutine(int trackIndex)
	{
		//FadeAll();
		currentTrackIndex = trackIndex;
		for (int i = 0; i < 5; ++i)
		{
			musicLayers[i].SetTrack(musicTracks[trackIndex].layers[i]);
			musicLayers[i].AudioSource.Play();
		}
		yield return null;
	}



	public void Update()
	{
		if (!listenersSet)
		{
			Tick.Instance.AddEventListener(SetLevel);
		}

		if (musicLayers[0].AudioSource.time >= musicTracks[currentTrackIndex].loopTime)
		{
			for(int i = 0; i < 5; ++i)
			{
				musicLayers[i].SwitchSource();
				musicLayers[i].AudioSource.Play();
			}
		}
	}

	public void SetLevel()
	{
		int level = Mathf.FloorToInt(Game.Instance.Simulation.currentState.Sustainability / 20);
		for(int i = 0; i< level; ++i)
		{
			if(musicLayers[i].AudioSource.volume != 1f)
			{
				musicLayers[i].FadeIn();
			}
		}
		for(int i = 4; i >= level; --i)
		{
			if (musicLayers[i].AudioSource.volume != 0f)
			{
				musicLayers[i].FadeOut();
			}
		}
	}

	public void FadeAll()
	{
		for (int i = 0; i < 5; ++i)
		{
			if (musicLayers[i].AudioSource.volume != 0f)
			{
				musicLayers[i].FadeOut();
			}
		}
	}
}
