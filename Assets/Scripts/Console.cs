using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
	public List<UILine> Lines;

    // Start is called before the first frame update
    void Awake()
    {
        //GameManager.Instance.Console = this;
    }

	public void AddStarter(LevelStarter ls) {
		
	}
	
	public void AddLine(int atIndex) {

	}

	public void HighlightLine(int lineNumber) {

	}

	

 //   public void Execute() {
	//	StartCoroutine(ExecuteCode());
	//}

	//private IEnumerator ExecuteCode() {
	//	foreach(Loop l in Loops) {
	//		yield return l.Execute();
	//	}
	//}
}
