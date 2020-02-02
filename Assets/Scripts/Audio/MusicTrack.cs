using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/MusicTrack", order = 1)]
public class MusicTrack : ScriptableObject
{
	public float loopTime;
	public AudioClip[] layers = new AudioClip[5];
}
