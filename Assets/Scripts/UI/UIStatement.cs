using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIStatement : UILine {
	public TMP_Dropdown ActionDropdown;
	public TMP_Dropdown AttributeDropdown;

	public override void Start()
    {
        base.Start();
		anim = GetComponent<Animator>();
	}

	public override string ToString() {
		InstructionEnum inst = (InstructionEnum)ActionDropdown.value;
		if(inst == InstructionEnum.MOVE) {
			return $"{(InstructionEnum)ActionDropdown.value}        {(DirectionEnum)AttributeDropdown.value}";
		}
		else {
			return $"{(InstructionEnum)ActionDropdown.value}";
		}
	}

	public void Init(int index, bool readOnly, InstructionComponent comp) {
		ActionDropdown.value = (int)comp.Instruction;
		AttributeDropdown.value = (int)comp.Direction;
		base.Init(index, readOnly);
	}

    public override Instruction GetInstruction()
    {
		InstructionEnum inst = (InstructionEnum)ActionDropdown.value;
		if(inst == InstructionEnum.MOVE) {
			return new MoveInstruction(((DirectionEnum)AttributeDropdown.value).GetDirectionValue(), LineNumber);
		}
		else {
			return new GrabInstruction(LineNumber);
		}
	}

	public void NewStatementSelected(int i) {
		InstructionEnum inst = (InstructionEnum)i;
		AttributeDropdown.gameObject.SetActive(inst == InstructionEnum.MOVE);
	}

	public override void ShowNewLine(bool show) {
		anim.SetBool("Show", show);
	}
}
