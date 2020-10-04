using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private Camera main;
	private LayerMask groundMask;

	private void Start() {
		main = Camera.main;
	}

	private void Update() {
		InputPackage p = new InputPackage();


		GameManager.Instance.ContextManager.HandleInput(p);
	}
}

public class InputPackage {
	public Vector3 MousePositionScreenSpace { get; set; }
	public Vector3 MousePositionWorldSpace { get; set; }
	public float MouseWheelDelta { get; set; }
	public bool LeftMouse { get; set; }
	public bool RightMouse { get; set; }

	public bool Enter { get; set; }
	public bool Drop { get; set; }
	public bool Dash { get; set; }
	public bool Jump { get; set; }
	public float Horizontal { get; set; }
	public float Vertical { get; set; }
}
