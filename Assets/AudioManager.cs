using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioSO m_audioSO;

    public PlayerSources player1Sources;
    public PlayerSources player2Sources;
    public AudioSource ambient;
    public AudioSource music;
    public AudioSource clash;

    public static AudioManager Instance;

    Dictionary<string, int> m_FXDictionary = new Dictionary<string, int>();


    private void Awake()
    {
        Instance = this;
        m_audioSO = Resources.Load<AudioSO>("AudioSO");
        for (int i = 0; i < m_audioSO.AudioFXList.Count; i++)
        {
            if (m_FXDictionary.ContainsKey(m_audioSO.AudioFXList[i].audioName))
            {
                Debug.LogError("audio error");
            }
            else
            {

                m_FXDictionary.Add(m_audioSO.AudioFXList[i].audioName, i);
            }
        }
    }

    public void PlayAmbient(string _name)
    {
        AudioClip clip  = GetFX(_name);
        ambient.clip = clip;
        ambient.Play();
    }

    public void PlayMusic(string _name)
    {
        AudioClip clip = GetFX(_name);
        music.clip = clip;
        music.Play();
    }

    public void PlayClash(string _name)
    {
        AudioClip clip = GetFX(_name);
        clash.clip = clip;
        clash.Play();
    }

    public void PlayPlayerFX(string _name, bool _isLeft)
    {
        PlayerSources sources = (_isLeft) ? player1Sources : player2Sources;
        AudioClip clip = GetFX(_name);

        if (!sources.source1.isPlaying)
        {
            sources.source1.clip = clip;
            sources.source1.Play();
        }
        else if (!sources.source2.isPlaying)
        {
            sources.source2.clip = clip;
            sources.source2.Play();
        }
        else if (!sources.source3.isPlaying)
        {
            sources.source3.clip = clip;
            sources.source3.Play();
        }
        else if (!sources.source4.isPlaying)
        {
            sources.source4.clip = clip;
            sources.source4.Play();
        }
        else
        {
            sources.source1.clip = clip;
            sources.source1.Play();
        }
    }
 

    public AudioClip GetFX(string name)
    {
        int index = StringToInt(m_FXDictionary, name);
        if (index == -1)
            return null;

        return m_audioSO.AudioFXList[index].clip;
    }

    int StringToInt(Dictionary<string, int> _dictionary, string _name)
    {
        int index = -1;
        _dictionary.TryGetValue(_name, out index);
        
        return index;
         
    }

   
}

[System.Serializable]
public class PlayerSources
{
    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;
    public AudioSource source4;
}
