using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThankYou : MonoBehaviour
{
	public TMP_Text Cursor;
	public TMP_Text Text;

	void Start()
    {
        StartCoroutine(Reveal());
    }

	public IEnumerator Reveal() {
		Animator anim = GetComponent<Animator>();
		yield return new WaitForSeconds(2f);
		Cursor.gameObject.SetActive(false);
		string text = "THANK YOU\nFOR PLAYING";
		string tmpText = "";
		var wait = new WaitForSeconds(0.075f);

		var audio = GetComponent<AudioSource>();
		foreach (char c in text) {
			audio.pitch = 0.9f + 0.2f * Random.value;
			audio.Play();

			tmpText += c;
			Text.text = tmpText;
			yield return wait;
		}

		Cursor.rectTransform.anchoredPosition = new Vector2(4f, -0.5f);
		Cursor.gameObject.SetActive(true);
	}
}
