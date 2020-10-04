using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExit : UILine
{
	public override Instruction GetInstruction() {
		return new ExitInstruction(LineNumber);
	}

	public override string ToString() {
		return "EXIT";
	}
}
