using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneHelpers {
	public static int SceneCount => SceneManager.sceneCountInBuildSettings;

	public static int GetNextLevelBuildIndex() {
		return GetCurrentLevelBuildIndex() + 1;
	}

	public static int GetCurrentLevelBuildIndex() {
		var buildIndex = 0;
		for (int i = 0; i < SceneManager.sceneCount; i++) {
			Scene s = SceneManager.GetSceneAt(i);
			if (s.isLoaded) {
				return s.buildIndex;
			}
		}
		return buildIndex;
	}
}

