using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    SerializedProperty soundPrefab;
    SerializedProperty maxPool;

    SerializedProperty defaultBGMVolume;
    SerializedProperty defaultAmbientVolume;
    SerializedProperty defaultSFXVolume;

    SerializedProperty bgm;
    SerializedProperty ambience;
    SerializedProperty sfx;


    private void OnEnable()
    {
        soundPrefab = serializedObject.FindProperty("soundPrefab");
        maxPool = serializedObject.FindProperty("maxPool");

        defaultBGMVolume = serializedObject.FindProperty("defaultBGMVolume");
        defaultAmbientVolume = serializedObject.FindProperty("defaultAmbientVolume");
        defaultSFXVolume = serializedObject.FindProperty("defaultSFXVolume");

        bgm = serializedObject.FindProperty("bgm");
        ambience = serializedObject.FindProperty("ambience");
        sfx = serializedObject.FindProperty("sfx");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(soundPrefab);
        EditorGUILayout.PropertyField(maxPool);

        EditorGUILayout.PropertyField(defaultBGMVolume);
        EditorGUILayout.PropertyField(defaultAmbientVolume);
        EditorGUILayout.PropertyField(defaultSFXVolume);

        GUIContent bgmLabel = new GUIContent("BGM");
        EditorGUILayout.PropertyField(bgm, bgmLabel, true);
        GUIContent ambientLabel = new GUIContent("Ambience");
        EditorGUILayout.PropertyField(ambience, ambientLabel, true);
        GUIContent sfxLabel = new GUIContent("SFX");
        EditorGUILayout.PropertyField(sfx, sfxLabel, true);

        serializedObject.ApplyModifiedProperties();

        if (Application.isPlaying)
            (target as SoundManager).UpdateVolumes();
    }
}
