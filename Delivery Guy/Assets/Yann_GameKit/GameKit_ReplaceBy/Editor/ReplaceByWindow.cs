using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace GameKit
{
	public class ReplaceByWindow : EditorWindow
	{
		[MenuItem("ReplaceBy/Window")]
		public static void CreateEditorWindow( )
		{
			EditorWindow.GetWindow<ReplaceByWindow>(false, "Replace By", false);
		}

		static GameObject gameObjectModelReplacing;
		void OnGUI()
		{
			gameObjectModelReplacing = EditorGUILayout.ObjectField("Model", gameObjectModelReplacing, typeof(GameObject), true) as GameObject;

			List<GameObject> gameObjectsToReplace = new List<GameObject>();
			foreach(GameObject selectedGameObject in Selection.gameObjects)
			{
				if(EditorUtility.IsPersistent(selectedGameObject))
					continue;

				gameObjectsToReplace.Add(selectedGameObject);
			}
					
			GUI.enabled = gameObjectModelReplacing != null && gameObjectsToReplace.Count > 0;
			if(GUILayout.Button("Replace"))
			{
				List<GameObject> createdReplacingGameObjects = new List<GameObject>();
				ReplaceByUtility.ReplaceBy(gameObjectsToReplace, gameObjectModelReplacing,
				                           ref createdReplacingGameObjects);
				Selection.objects = createdReplacingGameObjects.ToArray();
			}

			if(GUILayout.Button("ReplaceWithScaleOverride"))
			{
				List<GameObject> createdReplacingGameObjects = new List<GameObject>();
				ReplaceByUtility.ReplaceBy(gameObjectsToReplace, gameObjectModelReplacing,
				                           ref createdReplacingGameObjects, true);
				Selection.objects = createdReplacingGameObjects.ToArray();
			}

            if (GUILayout.Button("ReplaceWithSameMaterial"))
            {
                List<GameObject> createdReplacingGameObjects = new List<GameObject>();
                ReplaceByUtility.ReplaceBy(gameObjectsToReplace, gameObjectModelReplacing,
                                           ref createdReplacingGameObjects, false, true);
                Selection.objects = createdReplacingGameObjects.ToArray();
            }
        }
	}
}