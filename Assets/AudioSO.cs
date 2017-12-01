using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[CreateAssetMenu]
public class AudioSO : ScriptableObject {

    public List<AudioFX> AudioFXList;

    //[MenuItem("Assets/AudioSO")]
    //public static void Create()
    //{
    //    Debug.Log("create1");
    //    AudioSO audioSO = CreateInstance<AudioSO>();
    //    AssetDatabase.CreateAsset(audioSO, "Assets/Resources/AudioSO.asset");
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

}


