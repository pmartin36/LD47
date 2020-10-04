using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinModal : MonoBehaviour
{
    public TMP_Text Cursor;
	public TMP_Text Text;
	private Animator anim;
	public bool AnimationCompleted;

	public void Start() {
		anim = GetComponent<Animator>();
	}

	public IEnumerator Reveal() {
		anim.SetFloat("direction", 1f);
		anim.Play("WinModal", 0, 0f);
		Cursor.text = "|";
		yield return new WaitForSeconds(0.5f);
		Cursor.gameObject.SetActive(false);
		string text = "EXIT CODE 0\nSUCCESS";
		string tmpText = "";
		var wait = new WaitForSeconds(0.075f);
		
		var audio = GetComponent<AudioSource>();
		foreach(char c in text) {
			audio.pitch = 0.6f + 0.2f * Random.value;
			audio.Play();

			tmpText += c;
			Text.text = tmpText;
			yield return wait;
		}

		Cursor.rectTransform.anchoredPosition = new Vector2(2.85f, -0.5f);
		Cursor.gameObject.SetActive(true);

		AnimationCompleted = true;
	}

	public IEnumerator Hide() {
		yield return new WaitForSeconds(0.5f);
		Cursor.text = "";
		anim.SetFloat("direction", -1f);
		anim.Play("WinModal", 0, 1f);
		yield return new WaitForSeconds(1.25f);
		GameManager.Instance.UnloadScene(this.gameObject.scene.buildIndex);
	}
}
