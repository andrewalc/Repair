using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
	public List<MusicTrack> musicTracks;
	public MusicLayer[] musicLayers = new MusicLayer[5];
	private int currentTrackIndex = 0;

	public static SoundManager Instance { get; private set; }

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		SetTrack(0);
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
		if (Input.GetKey(KeyCode.Alpha1))
		{
			SetLevel(1);
		}else if (Input.GetKey(KeyCode.Alpha2))
		{
			SetLevel(2);
		}
		else if (Input.GetKey(KeyCode.Alpha3))
		{
			SetLevel(3);
		}
		else if (Input.GetKey(KeyCode.Alpha4))
		{
			SetLevel(4);
		}
		else if (Input.GetKey(KeyCode.Alpha5))
		{
			SetLevel(5);
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

	public void SetLevel(int level)
	{
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
