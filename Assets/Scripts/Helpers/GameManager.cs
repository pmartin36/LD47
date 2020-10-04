using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : Singleton<GameManager> {

	private float _timeScale = 1f;
	private float _targetTimeScale = 1f;
	private float timeLerpScale;
	private float TimeScale {
		get => _timeScale;
		set {
			Time.timeScale = value;
			_timeScale = value;
		}
	}

	public ContextManager ContextManager;
	public LevelManager LevelManager {
		get => ContextManager as LevelManager;
		set => ContextManager = value;
	}

	public float SFXVolume;

	public bool Paused => TimeScale < 0.9f;

	public void Awake() {
		ContextManager = GameObject.FindObjectOfType<ContextManager>();
	}

	public void Update() {
		TimeScale = Mathf.Lerp(TimeScale, _targetTimeScale, timeLerpScale);
	}

	public void HandleInput(InputPackage p) {
		ContextManager.HandleInput(p);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void SetTimescale(float timescale, float lerpScale = 0.5f) {
		_targetTimeScale = timescale;
		timeLerpScale = lerpScale;
	}

	public void TogglePause() {
		TimeScale = Paused ? 1f : 0.00001f;
	}

	private IEnumerator LoadSceneAsync(int buildIndex, Coroutine waitUntil = null, CancellationTokenSource cts = null, Action onSceneSwitch = null, bool shouldUnloadCurrentScene = true) {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

		asyncLoad.allowSceneActivation = false;

		if(waitUntil != null) {
			yield return waitUntil;
		}
		yield return new WaitUntil(() => asyncLoad.progress >= 0.9f); //when allowsceneactive is false, progress stops at .9f

		if(cts != null && cts.IsCancellationRequested) {
			asyncLoad.allowSceneActivation = true;
			yield return new WaitUntil(() => asyncLoad.isDone);
			SceneManager.UnloadSceneAsync(buildIndex);
		}
		else {
			int currentScene = SceneHelpers.GetCurrentLevelBuildIndex();

			asyncLoad.allowSceneActivation = true;
			yield return new WaitUntil(() => asyncLoad.isDone);

			onSceneSwitch?.Invoke();

			if (shouldUnloadCurrentScene) {
				SceneManager.UnloadSceneAsync(currentScene);
			}
		}
	}

	public void AsyncLoadScene(int buildIndex, Coroutine waitUntil = null, CancellationTokenSource cts = null, Action onSceneSwitch = null, bool shouldUnloadCurrentScene = true) {
		StartCoroutine(
			LoadSceneAsync(
				buildIndex,
				waitUntil,
				cts,
				onSceneSwitch,
				shouldUnloadCurrentScene
			)
		);
	}	

	public void LoadScene(int buildIndex, Coroutine waitUntil = null, Action onSceneSwitch = null) {
		ShowLoadScreen(true);
		StartCoroutine(
			LoadSceneAsync(
				buildIndex, 
				StartCoroutine(LoadWrapper(waitUntil)), // wait for at least 1 seconds + waituntil
				null, 
				() => {
					onSceneSwitch?.Invoke();		
					ShowLoadScreen(false);
				}
			)
		);	
	}

	public void UnloadScene(int buildIndex, Action<AsyncOperation> callback = null) {
		AsyncOperation unload = SceneManager.UnloadSceneAsync(buildIndex);
		if(callback != null) {
			unload.completed += callback;
		}
	}

	public void ShowLoadScreen(bool show, bool showLoadTiles = true) {
		//if(loadScreen == null) {
		//	loadScreen = FindObjectOfType<LoadScreen>();
		//}

		//loadScreen.Show(show, showLoadTiles);
	}

	public IEnumerator LoadWrapper(Coroutine waitUntil = null) {
		yield return new WaitForSeconds(1f);
		if(waitUntil != null) {
			yield return waitUntil;
		}
	}

	public void AdjustAudio(SoundType t, float value) {
		if(t == SoundType.SFX) {
			SFXVolume = value;
		}
		else {
			MusicManager.Instance.SetVolume(value);
		}
	}
}