using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPointer : MonoBehaviour
{
	public int ShowOrder;
	public float ShowTime = 3f;
    public Image Arrow;
	public TMP_Text HelpText;

    public void SetAlpha(float alpha)
    {
        var color = Colors.Cream;
		color.a = alpha;
		if(Arrow != null)
			Arrow.color = color;
		if(HelpText != null)
			HelpText.color = color;
    }
}
