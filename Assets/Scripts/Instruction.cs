using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Instruction {
	public int CorrespondingUILineNumber { get; set; }
	public int? DecrementForLoopOnLine { get; set; }
	public abstract Coroutine Execute(Player p);
	public Instruction(int ln) {
		CorrespondingUILineNumber = ln;
	}
	public bool IsLoopBreaker => (this is LoopInstruction) || (this is ExitInstruction);
}

public class MoveInstruction : Instruction {
	public Direction Direction { get; set; }

	public MoveInstruction(Direction d, int ln) : base(ln) {
		Direction = d;
	}
	public override Coroutine Execute(Player p) {
		return p.Move(Direction);
	}
}

public class GrabInstruction : Instruction {
	public GrabInstruction(int ln) : base(ln) {}
	public override Coroutine Execute(Player p) {
		return p.Grab();
	}
}

public class ExitInstruction : Instruction {
	public ExitInstruction(int ln) : base(ln) {}
	public override Coroutine Execute(Player p) {
		return p.Exit();
	}
}

public class LoopInstruction : Instruction {
	public int End { get; set; }
	public LoopInstruction(int end, int ln) : base(ln) {
		End = end;
	}
	public override Coroutine Execute(Player p) => null;
}