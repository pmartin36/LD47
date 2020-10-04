using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstructionComponent", menuName = "ScriptableObjects/InstructionComponent", order = 1)]
public class InstructionComponent : ScriptableObject {
	public InstructionEnum Instruction;
	public DirectionEnum Direction;
}