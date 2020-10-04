using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCreator : MonoBehaviour
{
	public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
		MusicManager.Instance.SetTrack(clip);
		MusicManager.Instance.SetVolume(0.2f);
	}
}
