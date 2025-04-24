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

		// Conditional fields:
		if (weaponType == WeapClass.WeaponType.gun)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
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
			var throwTypeProp = serializedObject.FindProperty("throwableType");
			EditorGUILayout.PropertyField(throwTypeProp);
			WeapClass.ThrowType throwType = (WeapClass.ThrowType)throwTypeProp.enumValueIndex;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("throwSpeed"));

			if (throwType == WeapClass.ThrowType.bottle)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
			}

			

		}
		else if (weaponType == WeapClass.WeaponType.melee)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("firingError"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("damageFalloffRange"));
		}

		// Always show:
		EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSound"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadSound"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("emptySound"));

		serializedObject.ApplyModifiedProperties();
	}
}
