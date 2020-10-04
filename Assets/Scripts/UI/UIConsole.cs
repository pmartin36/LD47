using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIConsole : MonoBehaviour
{
	private List<UILine> Lines;

	public UIStatement StatementPrefab;
	public UILoop LoopPrefab;
	public UIBlankLine BlankLinePrefab;
	public UIExit ExitPrefab;

    public UILine HoveredLine { get; set; }
	private float GetLinePosition(int i) => -7 - i * 14;

	private UILine HighlightedLine { get; set; }

	private Transform LineContainer;

	public void Start() {
		GameManager.Instance.LevelManager.Console = this;
	}

	public void AddStarter(LevelStarter ls) {
		int index = 0;
		Lines = new List<UILine>();
		LineContainer = this.transform.GetChild(0);
		foreach(var loopInfo in ls.Loops) {
			UILoop loop = Instantiate(LoopPrefab, LineContainer);
			loop.Init(index++, true, loopInfo.End);
			Lines.Add(loop);
			foreach(var instruction in loopInfo.StarterInstructions) {
				UIStatement statement = Instantiate(StatementPrefab, LineContainer);
				statement.Init(index++, true, instruction);
				Lines.Add(statement);
			}
			for(int i = loopInfo.StarterInstructions.Count; i < loopInfo.MaxInstructions; i++) {
				UIBlankLine line = Instantiate(BlankLinePrefab, LineContainer);
				line.Init(index++, false);
				Lines.Add(line);
			}

			// insert empty line between loops
			index++;
		}
		UIExit exit = Instantiate(ExitPrefab, LineContainer);
		exit.Init(index++, true);
		Lines.Add(exit);
	}

	public void AddNewLine(UILine line) {
		int index = Lines.IndexOf(line);

		var newline = Instantiate(StatementPrefab, LineContainer);
		newline.Init(line.LineNumber, false);

		Lines[index].gameObject.SetActive(false);
		Lines[index].Destroy();
		Lines[index] = newline;
	}

	public void BlankLine(UILine line) {
		int index = Lines.IndexOf(line);

		UILine blankLine = Instantiate(BlankLinePrefab, LineContainer);
		blankLine.Init(line.LineNumber, false);

		Lines[index].gameObject.SetActive(false);
		DestroyImmediate(Lines[index]);
		Lines[index] = blankLine;
	}

	public void Reset() {
		SetEditable(true);
		foreach(UILoop l in Lines.Where(l => l is UILoop)) {
			l.ResetCount();
		}
		ClearHighlight();
	}

	public void HighlightLine(int ln) {
		ClearHighlight();

		var line = Lines.FirstOrDefault(l => l.LineNumber == ln);
		line.HighlightLine(true);
		HighlightedLine = line;
	}

	private void ClearHighlight() {
		if (HighlightedLine != null) {
			HighlightedLine.HighlightLine(false);
			HighlightedLine = null;
		}
	}

	public void HoveredOut() {
		HideHoveredLine();
	}

	public void LineUnhovered(UILine line) {
		if(HoveredLine == line) {
			HideHoveredLine();
		}
	}

	public void LineHovered(UILine line) {
		HideHoveredLine();
		if(!line.ReadOnly) {
			HoveredLine = line;
			HoveredLine.ShowNewLine(true);
		}
	}

	private void HideHoveredLine() {
		if (HoveredLine != null) {
			HoveredLine.ShowNewLine(false);
		}
	}

	public void Deselect() {
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void DecrementLoop(int ln) {
		var loop = Lines.FirstOrDefault(l => l.LineNumber == ln);
		if(loop != null && loop is UILoop) {
			(loop as UILoop).Decrement();
		}
	}

	public List<Instruction> GetInstructions() {
		return Lines
			.Select(l => l.GetInstruction())
			.Where(l => l != null)
			.ToList();
	}

	public void SetEditable(bool editable) {
		foreach(var line in Lines) {
			if(editable)
				line.SetEditable();
			else
				line.SetReadOnly();
		}
	}
}
