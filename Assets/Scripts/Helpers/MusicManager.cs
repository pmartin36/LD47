using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager> {

	private AudioSource audio;
	private float Volume;

	private int LastRequestedWorldMusic = -1;
	private Coroutine transitionTrackCoroutine;


	private void Init() {
		audio = audio ?? gameObject.AddComponent<AudioSource>();
		audio.loop = true;
	}

	public void SetVolume(float volume) {
		Volume = volume;
		audio.volume = volume;
	}

	public void SlideVolume(float volume, float t) {
		Volume = volume;
		StartCoroutine(SlideVolumeRoutine(Volume, t));
	}

	public void SetTrack(AudioClip track) {
		Init();
		audio.clip = track;
		audio.Play();
	}

	private IEnumerator SlideVolumeRoutine(float v, float t) {
		float startVolume = audio.volume;
		float elapsed = 0f;
		while(elapsed < t) {
			audio.volume = Mathf.Lerp(startVolume, v, elapsed / t);
			elapsed += Time.deltaTime;
			yield return null;
		}
		audio.volume = v;
	}

	private IEnumerator TransitionTrack(AudioClip track, float time, float finalVolume) {
		float animationTime = time / 2f;
		float elapsedTime = 0f;
		float startVolume = audio.volume;
		float endVolume = 0f;

		System.Action<float> changeVolumeLoop = (t) => {
			float v = Mathf.Lerp(startVolume, endVolume, t);
			SetVolume(v);
			elapsedTime += Time.deltaTime;
		};

		while( elapsedTime < animationTime ) {
			changeVolumeLoop(elapsedTime/animationTime);
			yield return null;
		}
		SetVolume(0f);
		yield return new WaitForSeconds(0.5f);
		SetTrack(track);
		audio.Play();

		elapsedTime = 0f;
		startVolume = 0f;
		endVolume = finalVolume;
		while (elapsedTime < animationTime) {
			changeVolumeLoop(elapsedTime / animationTime);
			yield return null;
		}
		SetVolume(finalVolume);
	}
}
