using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelStarter", menuName = "ScriptableObjects/LevelStarter", order = 1)]
public class LevelStarter : ScriptableObject {
	public List<LoopComponent> Loops;
}


