using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPointerManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ShowPointers());
    }

    private IEnumerator ShowPointers() {
		float transitionTime = 0.5f;
		
		var pointerGroups = GetComponentsInChildren<UIPointer>().GroupBy(p => p.ShowOrder).OrderBy(p => p.Key);
		foreach(var pGroup in pointerGroups) {
			float t = 0f;

			while(t < transitionTime) {
				foreach(UIPointer p in pGroup) {
					p.SetAlpha(t / transitionTime);
				}
				t += Time.deltaTime;
				yield return null;
			}
			t = 0f;
			yield return new WaitForSeconds(pGroup.First().ShowTime);
			while (t < transitionTime) {
				foreach (UIPointer p in pGroup) {
					p.SetAlpha(1 - (t / transitionTime));
				}
				t += Time.deltaTime;
				yield return null;
			}

			foreach (UIPointer p in pGroup) {
				p.SetAlpha(0);
			}
		}
	}
}
