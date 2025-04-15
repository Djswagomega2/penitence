using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeapClass))]
public class WeaponDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var weaponTypeProp = serializedObject.FindProperty("weaponType");
		EditorGUILayout.PropertyField(weaponTypeProp);

		WeapClass.WeaponType weaponType = (WeapClass.WeaponType)weaponTypeProp.enumValueIndex;

		// Always show:
		EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSound"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadSound"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("emptySound"));

		// Conditional fields:
		if (weaponType == WeapClass.WeaponType.gun)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ammoCapacity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_fireRate"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("firingError"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("damageFalloffRange"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("damageFalloff"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadSpeed"));
		}
		else if (weaponType == WeapClass.WeaponType.throwable)
		{
			//Fix it so that if throwable type is bottle it shows weapon damage
			EditorGUILayout.PropertyField(serializedObject.FindProperty("throwableType"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("throwSpeed"));
		}
		else if (weaponType == WeapClass.WeaponType.melee)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("firingError"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("damageFalloffRange"));
		}

		serializedObject.ApplyModifiedProperties();
	}
}
