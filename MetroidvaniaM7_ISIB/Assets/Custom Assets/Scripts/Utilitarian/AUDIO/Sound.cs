using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public AudioClip AudioClip;
    public List<_tag> Tag = new List<_tag>();

    public Sound(AudioClip inClip)
    {
        AudioClip = inClip;
    }
    

    public enum _tag
    {
        CALM,
        DANGER,
        DISCOVERY,
        FAST
    }
}
