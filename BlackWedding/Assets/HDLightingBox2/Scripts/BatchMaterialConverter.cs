
// AliyerEdon@gmail.com/
// Lighting Box is an "paid" asset. Don't share it for free

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System;

public enum MatType
{
	HDLit,
	Standard,Lightweight,
	Custom
}
public enum MaterialConverterType
{
	CustomMaterials,
}
public class BatchMaterialConverter : EditorWindow {

	[MenuItem("Window/Batch Material Converter")]
	static void Init()
	{
		BatchMaterialConverter window = (BatchMaterialConverter)EditorWindow.GetWindow(typeof(BatchMaterialConverter));
		window.Show();
		window.autoRepaintOnSceneChange = true;
	}

	Vector2 scrollPos;

	public MaterialConverterType converterType;
	public string[] CustomPath = {"Assets/MobileOptimizer/Materials"};
	public Material[] targets;
	MatType matType;
	Material m ;
	string shaderName = "HDRenderPipeline/Unlit";
		
	void OnGUI()
	{
		Undo.RecordObject (this,"lb");

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos,
			false,
			false,
			GUILayout.Width(Screen.width ),   
			GUILayout.Height(Screen.height));
		
		EditorGUILayout.Space ();EditorGUILayout.Space ();
		EditorGUILayout.Space ();EditorGUILayout.Space ();

		converterType = (MaterialConverterType)EditorGUILayout.EnumPopup("Importer Mode",converterType,GUILayout.Width(343));
		matType = (MatType)EditorGUILayout.EnumPopup("Target Shader",matType,GUILayout.Width(343));

		if (matType == MatType.Custom)
			shaderName = EditorGUILayout.TextField("Custom Shader",shaderName,GUILayout.Width(343));
		
		if (GUILayout.Button ("Batch Convert")) {

			if (converterType == MaterialConverterType.CustomMaterials) {
				if (targets.Length > 0) {
					foreach (Material s in targets) {
						if (matType == MatType.Standard)
							s.shader = Shader.Find ("Standard");
						if (matType == MatType.HDLit)
							s.shader = Shader.Find ("HDRenderPipeline/Lit");
						if (matType == MatType.Lightweight)
							s.shader = Shader.Find ("Lightweight Render Pipeline/Lit");
						if (matType == MatType.Custom)
							s.shader = Shader.Find (shaderName);
					}
				}
			}
		}
		EditorGUILayout.Space ();EditorGUILayout.Space ();

		if (converterType == MaterialConverterType.CustomMaterials) 
		{
			ScriptableObject target = this;
			SerializedObject so = new SerializedObject(target);
			SerializedProperty stringsProperty = so.FindProperty("targets");
			EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
			so.ApplyModifiedProperties(); // Remember to apply modified properties
		}


		EditorGUILayout.Space ();EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.EndScrollView();

	}
}
#endif

