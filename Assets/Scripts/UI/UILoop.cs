using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoop : UILine {
    public int End { get; set; }
	public int Count { get; set; }

	public override void Start() {
		base.Start();
	}

	public void Init(int index, bool readOnly, int end) {
		End = end;
		Count = End;
		base.Init(index, readOnly);
	}

	public void ResetCount() {
		Count = End;
		ReadOnlyText.text = this.ToString();
	}

	public void Decrement() {
		Count--;
		ReadOnlyText.text = this.ToString();
	}

	public override Instruction GetInstruction() {
		return new LoopInstruction(End, LineNumber);
	}

	public override string ToString() {
		if(Count != 1) {
			return $"REPEAT {Count} TIMES:";
		}
		else {
			return $"REPEAT 1 TIME:";
		}
	}

	public void NewLine() => base.AddNewLine();
}
