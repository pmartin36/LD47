using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(InputManager))]
public class LevelManager : ContextManager
{
	private Camera main;
	private InputPackage lastInput;
	public LevelStarter levelStarter;

	public bool Playing { get; set; }
	public Player Player { get; set; }
	public UIConsole Console { get; set; }
	public UIButtons Buttons { get; set; }

	private Coroutine RunRoutine;

	public override void Awake() {
		base.Awake();
		main = Camera.main;
	}

	public override void Start()
    {
		FindObjectOfType<UIConsole>().AddStarter(levelStarter);
    }

	public void PlayPause() {
		GameManager gm = GameManager.Instance;
		if (Playing) {
			gm.TogglePause();
		}
		else {
			Console.SetEditable(false);
			List<Instruction> instructions = Console.GetInstructions();
			for (int i = 0; i < instructions.Count; i++) {
				ParseInstruction(instructions, ref i);
			}

			Playing = true;
			Buttons.SetRunning();

			// Run
			RunRoutine = StartCoroutine(Run(instructions));
		}
	}

	private Instruction ParseInstruction(List<Instruction> instructions, ref int i) {
		var instruction = instructions[i++];
		if(instruction is LoopInstruction) {
			List<Instruction> loopedInstructions = new List<Instruction>();

			//Instruction nextInstruction = ParseInstruction(instructions, ref i);
			//while(!nextInstruction.IsLoopBreaker) {
			//	loopedInstructions.Add(nextInstruction);
			//	nextInstruction = ParseInstruction(instructions, ref i);
			//}

			Instruction nextInstruction = instructions[i++];
			while(!nextInstruction.IsLoopBreaker) {
				loopedInstructions.Add(nextInstruction);
				nextInstruction = instructions[i++];
			}

			if (loopedInstructions.Count > 0) {
				var lastInstruction = loopedInstructions.Last();
				lastInstruction.DecrementForLoopOnLine = instruction.CorrespondingUILineNumber;
			}

			instructions.Remove(instruction);
			i -= 2;
			LoopInstruction loopInstruction = instruction as LoopInstruction;
			for(int j = 1; j < loopInstruction.End; j++) {
				instructions.InsertRange(i, loopedInstructions);
				i += loopedInstructions.Count;
			}
			i--;
		}
		return instruction;
	}

	public void StopRun() {
		GameManager gm = GameManager.Instance;
		Playing = false;
		if(gm.Paused) {
			gm.TogglePause();
		}

		Console.Reset();
		Player.Reset();
		Buttons.Reset();
	}

	public override void HandleInput(InputPackage p) {
	
	}

	private IEnumerator Run(List<Instruction> instructions) {
		var smallWait = new WaitForSeconds(Player.ActionTime / 2f);
		foreach (Instruction i in instructions) {
			Console.HighlightLine(i.CorrespondingUILineNumber);
			yield return i.Execute(Player);
			if(i.DecrementForLoopOnLine != null) {
				Console.DecrementLoop(i.DecrementForLoopOnLine.Value);
			}
			yield return smallWait;
		}
		
		if(Player.ReachedExit) {
			var winModal = FindObjectOfType<WinModal>();
			int index = SceneHelpers.GetCurrentLevelBuildIndex();
			GameManager.Instance.AsyncLoadScene(
				index + 1,
				StartCoroutine(winModal.Reveal()),
				onSceneSwitch: () => {
					StartCoroutine(winModal.Hide());
					Console.transform.parent.gameObject.SetActive(false);
					Player.transform.parent.gameObject.SetActive(false);
				},
				shouldUnloadCurrentScene: false);
		}
		else {
			StopRun();
		}
	}
}
