using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageSaver))]
public class ImageEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		using (new EditorGUI.DisabledScope(!Application.isPlaying))
		{
			if (GUILayout.Button("Save Dataset"))
			{
                ImageSaver trainer = (ImageSaver)target;
                trainer.Save();
            }
		}
	}
}
