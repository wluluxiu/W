using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class DependencyOrganizer : EditorWindow
{
	private Object selectedObject;
	
	private void OnGUI ()
	{
		GUILayout.Label("Select a Prefab or Scene", EditorStyles.boldLabel);
		selectedObject = EditorGUILayout.ObjectField("Prefab/Scene", selectedObject, typeof (Object), false);
		
		if (GUILayout.Button("Organize Dependencies"))
		{
			if (selectedObject != null)
			{
				var path = AssetDatabase.GetAssetPath(selectedObject);
				OrganizeDependencies(path);
			}
			else
			{
				Debug.LogWarning("Please select a prefab or scene.");
			}
		}
	}
	
	[MenuItem("Tools/Dependency Organizer")]
	public static void ShowWindow ()
	{
		GetWindow<DependencyOrganizer>("Dependency Organizer");
	}
	
	private void OrganizeDependencies (string assetPath)
	{
		var dependencies = AssetDatabase.GetDependencies(assetPath, true);
		var categorizedDependencies = new Dictionary<string, List<string>>();
		
		foreach (var dependency in dependencies)
		{
			if (dependency == assetPath) continue; // Skip the selected asset itself
			
			var extension = Path.GetExtension(dependency).ToLower();
			if (extension == ".png" || extension == ".jpg")
				extension = "texture";
			if (!categorizedDependencies.ContainsKey(extension))
				categorizedDependencies[extension] = new List<string>();
			categorizedDependencies[extension].Add(dependency);
		}
		
		var assetDirectory = Path.GetDirectoryName(assetPath);
		var assetName = Path.GetFileNameWithoutExtension(assetPath);
		var mainFolder = Path.Combine(assetDirectory, assetName);
		
		if (!Directory.Exists(mainFolder))
			Directory.CreateDirectory(mainFolder);
		
		foreach (var category in categorizedDependencies)
		{
			var categoryFolder = Path.Combine(mainFolder, category.Key.TrimStart('.'));
			if (!Directory.Exists(categoryFolder))
				Directory.CreateDirectory(categoryFolder);
			
			foreach (var dependency in category.Value)
			{
				var fileName = Path.GetFileName(dependency);
				var destinationPath = Path.Combine(categoryFolder, fileName);
				
				if (category.Key == ".controller")
					MoveControllerAnimations(dependency, mainFolder);
				
				AssetDatabase.MoveAsset(dependency, destinationPath);
			}
		}
		
		AssetDatabase.Refresh();
		Debug.Log("Dependencies organized successfully.");
	}
	
	private void OrganizeDependencies1 (string assetPath)
	{
		var dependencies = AssetDatabase.GetDependencies(assetPath, true);
		var categorizedDependencies = new Dictionary<string, List<string>>();
		
		foreach (var dependency in dependencies)
		{
			if (dependency == assetPath) continue; // Skip the selected asset itself
			
			var extension = Path.GetExtension(dependency).ToLower();
			if (extension == ".png")
				extension = "texture";
			if (!categorizedDependencies.ContainsKey(extension))
				categorizedDependencies[extension] = new List<string>();
			categorizedDependencies[extension].Add(dependency);
		}
		
		var assetDirectory = Path.GetDirectoryName(assetPath);
		foreach (var category in categorizedDependencies)
		{
			var categoryFolder = Path.Combine(assetDirectory, category.Key.TrimStart('.'));
			if (!Directory.Exists(categoryFolder))
				Directory.CreateDirectory(categoryFolder);
			
			foreach (var dependency in category.Value)
			{
				var fileName = Path.GetFileName(dependency);
				var destinationPath = Path.Combine(categoryFolder, fileName);
				
				if (category.Key == ".controller")
					MoveControllerAnimations(dependency, assetDirectory);
				else
					AssetDatabase.MoveAsset(dependency, destinationPath);
			}
		}
		
		AssetDatabase.Refresh();
		Debug.Log("Dependencies organized successfully.");
	}
	
	private void MoveControllerAnimations (string controllerPath, string assetDirectory)
	{
		var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
		if (controller == null) return;
		
		var animationsFolder = Path.Combine(assetDirectory, "animation");
		if (!Directory.Exists(animationsFolder))
			Directory.CreateDirectory(animationsFolder);
		
		foreach (var layer in controller.layers)
		{
			foreach (var state in layer.stateMachine.states)
			{
				var motion = state.state.motion;
				if (motion is AnimationClip)
				{
					var animationPath = AssetDatabase.GetAssetPath(motion);
					var animationFileName = Path.GetFileName(animationPath);
					var destinationPath = Path.Combine(animationsFolder, animationFileName);
					AssetDatabase.MoveAsset(animationPath, destinationPath);
				}
			}
		}
	}
}