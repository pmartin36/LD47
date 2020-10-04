using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectDropdown : MonoBehaviour
{
    public void ClearSelect() {
		EventSystem.current.SetSelectedGameObject(null);
	}
}
