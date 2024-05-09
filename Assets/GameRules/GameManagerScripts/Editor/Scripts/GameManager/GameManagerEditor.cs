using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.ComponentModel;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    GameManager m_GameManager;
    private void OnSceneGUI()
    {
        m_GameManager = (GameManager)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Audio Scource Helper"))
        {
            m_GameManager.GenerateAudioHelper();
        }
    }
}
