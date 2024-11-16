using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class AudioManage : MonoBehaviour
{
    public Dictionary<string, AudioClip> audio_lib = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        instance = this;
        var audios = Resources.LoadAll<AudioClip>("Audio");
        foreach (var a in audios)
        {
            audio_lib.Add(a.name, a);
        }
    }

    public void play_audio(string name, Transform pos = null)
    {
        if (audio_lib.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(audio_lib[name], pos == null ? Camera.main.transform.position : pos.position);
        }
    }

    AudioManage() { }
    static AudioManage instance;
    public static AudioManage Instance
    {
        get
        {
            return instance;
        }
    }
}
