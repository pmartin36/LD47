using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : CodeElement
{
	public int End;
	public List<Line> Lines;

	public override Coroutine Execute() {
		return StartCoroutine(ExecuteCode());
	}

	private IEnumerator ExecuteCode() {
		for(int i = 0; i < End; i++) {
			HighlightLine();
			yield return OneSecond;
			foreach(Line line in Lines) {
				yield return line.Execute();
			}
		}
	}
}
