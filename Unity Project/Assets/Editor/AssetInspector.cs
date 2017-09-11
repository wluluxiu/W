//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using DG.Tweening;
//
//[CustomEditor(typeof(CustomTween))]
//public class BulletInspector : Editor {
//
//	public SerializedProperty tweenType;
//	public SerializedProperty tweener;
//
//	private void OnEnable()
//	{
//		tweenType = serializedObject.FindProperty("tweenType");
//		tweener = serializedObject.FindProperty("tweener");
//	}
//
//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//
//		EditorGUI.indentLevel = 1;
//
//		EditorGUILayout.PropertyField(tweenType, new GUIContent("动画类型"));
//		GUILayout.Space(5);
//
//		EditorGUILayout.PropertyField(tweener, new GUIContent("动画"));
//		GUILayout.Space(5);
//
//		// 打印数据
//		if (GUILayout.Button("Debug"))
//		{
//			Debug.Log("tweenType    :" + (TweenType)tweenType.enumValueIndex);
//			if (tweener.objectReferenceValue)
//			{
//				Debug.Log("effectObj    :" + tweener.objectReferenceValue);
//			}
//		}
//
//		if (GUI.changed)
//		{
//			EditorUtility.SetDirty(target);
//		}
//
//		serializedObject.ApplyModifiedProperties();
//	}
//
//}
