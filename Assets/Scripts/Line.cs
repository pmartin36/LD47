using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Line : CodeElement {
	public Instruction Instruction { get; set; }

    public override Coroutine Execute() {
		return StartCoroutine(ExecuteCode());
	}

	private IEnumerator ExecuteCode() {
		//highlight line about to execute for one second
		HighlightLine();
		yield return OneSecond;
		//Instruction.Execute();
	}
}
