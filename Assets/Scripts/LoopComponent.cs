using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LoopComponent", menuName = "ScriptableObjects/LoopComponent", order = 1)]
public class LoopComponent : ScriptableObject {
	public int End;
	public int MaxInstructions;
	public List<InstructionComponent> StarterInstructions;
}
