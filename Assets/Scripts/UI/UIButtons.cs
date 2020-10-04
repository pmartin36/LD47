using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
	public Sprite PauseSprite;
	public Sprite PlaySprite;

	public Image PlayPauseButton;
	public Button StopButton;

	public void Start() {
		GameManager.Instance.LevelManager.Buttons = this;
	}

	public void PlayPause() {
		GameManager.Instance.LevelManager.PlayPause();
		if(GameManager.Instance.Paused) {
			PlayPauseButton.sprite = PlaySprite;
		}
		else {
			PlayPauseButton.sprite = PauseSprite;
		}
	}

	public void SetRunning() {
		StopButton.interactable = true;
	}

	public void StopRun() {
		GameManager.Instance.LevelManager.StopRun();
	}

	public void Reset() {
		PlayPauseButton.sprite = PlaySprite;
		StopButton.interactable = false;
	}
}
