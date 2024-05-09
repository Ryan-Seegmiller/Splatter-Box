using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CreateAssetMenu(fileName = "New ReferenceAudio", menuName = "GameData/ReferenceValues/ReferenceAudio")]
public class ReferenceAudio : ScriptableObject
{
    [Serializable]
    public struct AudioClips
    {
        public string name;
        public int index;
        public AudioClip clip;
        public AudioSourceHelper.AudioClipType audioClipType;

        /// <summary>
        /// Starts the preview
        /// </summary>
       #if UNITY_EDITOR
       public void Preview()
        {
            var audio =  EditorGUIUtility.Load(AssetDatabase.GetAssetPath(clip));//Gets the file path of the clip and turns into a audioclip using the editor
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;//Gets the assembly in which the audio importer is
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");//Gets the class that is of type AudioUtil

            //Finds the method in which it can play a cound in the editor because for some bizzare reason this isnt publicly accesible????
            MethodInfo method = audioUtilClass.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null, new Type[] {typeof(AudioClip), typeof(Int32), typeof(bool)}, null);

            method.Invoke(null, new object[] { audio, 0, false });//Invokes the method
            
        }
        ///Stops the preview
        public void StopPreview()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;//Gets the assembly in which the audio importer is
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");//Gets the class that is of type AudioUtil

           
            //Finds the method in which it can play a cound in the editor because for some bizzare reason this isnt publicly accesible????
            MethodInfo method = audioUtilClass.GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public, null, new Type[] {}, null);

            method.Invoke(null, null);//Invokes the method

        }
        public void TypeCheck(AudioSourceHelper.AudioClipType type)
        {
            string asset = AssetDatabase.GetAssetPath(clip);
            AudioImporter importer = AssetImporter.GetAtPath(asset) as AudioImporter;
            switch (type)
            {
                case AudioSourceHelper.AudioClipType.SFX:
                    importer.forceToMono = true;
                    AssetDatabase.ImportAsset(asset);
                    break;
                case AudioSourceHelper.AudioClipType.DX:
                    importer.forceToMono = true;
                    AssetDatabase.ImportAsset(asset);
                    break;
                case AudioSourceHelper.AudioClipType.MX:
                    importer.forceToMono = false;
                    AssetDatabase.ImportAsset(asset);
                    break;
                case AudioSourceHelper.AudioClipType.FOL:
                    importer.forceToMono = true;
                    AssetDatabase.ImportAsset(asset);
                    break;
                case AudioSourceHelper.AudioClipType.BG:
                    importer.forceToMono = false;
                    AssetDatabase.ImportAsset(asset);
                    break;
                default: 
                    throw new ArgumentException("Not implemented audio clip type");
            }
        }
        #endif

    }

    public AudioClips[] clips;
}

