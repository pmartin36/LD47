using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlankLine : UILine
{
	public override Instruction GetInstruction() {
		return null;
	}

	public override string ToString() {
		return string.Empty;
	}

	public override void Start() {
		base.Start();
	}
}
