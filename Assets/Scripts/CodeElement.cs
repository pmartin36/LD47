using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CodeElement : MonoBehaviour
{
	public int LineNumber { get; set; }
    public static YieldInstruction OneSecond = new WaitForSeconds(1);
	public abstract Coroutine Execute();

	public bool ReadOnly { get; set; }
	public bool NewLineAvailable { get; set; }

	public void Start() {
		
	}

	public void HighlightLine() {
		// GameManager.Instance.Console.HighlightLine(LineNumber);
	}
}
