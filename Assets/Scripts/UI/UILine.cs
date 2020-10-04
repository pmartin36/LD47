using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class UILine : MonoBehaviour
{
	protected Animator anim;
	public int LineNumber { get; set; }

	public bool TemporaryReadOnly { get; set; }
	public bool AlwaysReadOnly { get; set; }
	public bool ReadOnly => TemporaryReadOnly || AlwaysReadOnly;

	protected Image LineImage;
	[SerializeField]
	protected TMP_Text ReadOnlyText;
	[SerializeField]
	protected Image Edit;

	public Image LineControl;
	protected RectTransform rt;

	public virtual void Start() {
		anim = GetComponent<Animator>();
	}

	protected float LinePosition => -7 - LineNumber * 14;
	public virtual void Init(int index, bool readOnly) {
		AlwaysReadOnly = readOnly;
		LineNumber = index;

		LineImage = GetComponent<Image>();

		rt = GetComponent<RectTransform>();
		var pos = rt.anchoredPosition;
		pos.y = LinePosition;
		rt.anchoredPosition = pos;

		if(AlwaysReadOnly) {
			ReadOnlyText.gameObject.SetActive(true);
			ReadOnlyText.text = this.ToString();
			ReadOnlyText.color = Colors.DarkCream;
			Edit.gameObject.SetActive(false);
		}
		else {
			ReadOnlyText.gameObject.SetActive(false);
			Edit.gameObject.SetActive(true);
		}
	}

	public abstract Instruction GetInstruction();
	public void SetReadOnly() {
		if(!AlwaysReadOnly) {
			ReadOnlyText.text = this.ToString();
			Edit.gameObject.SetActive(false);
			ReadOnlyText.gameObject.SetActive(true);
		}
		ReadOnlyText.color = Colors.Cream;
	}

	public void SetEditable() {
		LineImage.color = Color.clear;
		if(!AlwaysReadOnly) {
			ReadOnlyText.gameObject.SetActive(false);
			Edit.gameObject.SetActive(true);
		}
		else {
			ReadOnlyText.color = Colors.DarkCream;
		}
	}

	public void HighlightLine(bool shouldHighlight) {
		if(shouldHighlight) {
			ReadOnlyText.color = Colors.Black;
			LineImage.color = Colors.Cream;
		}
		else {
			ReadOnlyText.color = Colors.Cream;
			LineImage.color = Color.clear;
		}
	}

	public void AddNewLine() {
		EventSystem.current.SetSelectedGameObject(null);
		GameManager.Instance.LevelManager.Console.AddNewLine(this);
	}

	public void BlankLine() {
		EventSystem.current.SetSelectedGameObject(null);
		GameManager.Instance.LevelManager.Console.BlankLine(this);
	}

	public void HoverIn() {
		//GameManager.Instance.LevelManager.Console.LineHovered(this);
	}

	public void HoverOut() {
		//GameManager.Instance.LevelManager.Console.LineUnhovered(this);
	}

	public virtual void ShowNewLine(bool show) { }
}
