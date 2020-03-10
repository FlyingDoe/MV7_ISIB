using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Sound
{
    public AudioClip AudioClip;
    public List<_tag> Tag = new List<_tag>();

    public Sound(AudioClip inClip)
    {
        AudioClip = inClip;
        string name = inClip.name;

        Regex calm = new Regex(@"\bCALM\b");
        MatchCollection matches = calm.Matches(name);
        if (matches.Count > 0)
        {
            Tag.Add(_tag.CALM);
        }

        Regex danger = new Regex(@"\bDANGER\b");
        matches = danger.Matches(name);
        if (matches.Count > 0)
        {
            Tag.Add(_tag.DANGER);
        }

        Regex discovery = new Regex(@"\bDISCOVERY\b");
        matches = discovery.Matches(name);
        if (matches.Count > 0)
        {
            Tag.Add(_tag.DISCOVERY);
        }

        Regex fast = new Regex(@"\bFAST\b");
        matches = fast.Matches(name);
        if (matches.Count > 0)
        {
            Tag.Add(_tag.FAST);
        }




    }


    public enum _tag
    {
        CALM,
        DANGER,
        DISCOVERY,
        FAST
    }
}
