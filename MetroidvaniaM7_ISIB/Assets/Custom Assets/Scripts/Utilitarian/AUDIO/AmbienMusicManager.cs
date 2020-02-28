using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AmbienMusicManager : MonoBehaviour
{
    public AudioSource s;
    public AudioClip c;
    public double BPM = 100;
    public AudioMixer mixer;

    private double beatLength;
    private float timer;
    private float timerValue;

    private Sound._tag currentMood = Sound._tag.CALM;
    private tagFlag myTagFlag = tagFlag.MODIFIED;

    private AudioSource musicAudioSource;
    


    private int numberOfAudioFiles = 0;

    private List<Sound> backGroundSound = new List<Sound>();
    private List<Sound> soundsWhoMatchTheCurrentMood = new List<Sound>();

    void Awake()
    {
        //SETTING MUSIC BAR LENGHT
        double beatLength = 60.0 / 100.0;
        timerValue = (float)(beatLength * 4);
        timer = timerValue;

        //Getting the mixer 
        mixer = Resources.Load<AudioMixer>("mixer");

        //CREATING THE AUDIO SOURCE 
        musicAudioSource = GameObject.FindObjectOfType<Camera>().gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        musicAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BACKGROUND MUSIC")[0];

        //LOADING ALL MUSIC FILES IN THE DIRECTORY
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Custom Assets/Audio/Music/Resources");
        FileInfo[] info = dir.GetFiles("*.mp3*");


        foreach (FileInfo f in info)
        {
            if (f.Extension == ".mp3")
            {
                numberOfAudioFiles++;
                backGroundSound.Add(new Sound((AudioClip)Resources.Load(f.Name.Substring(0 ,f.Name.Length-4))));   
            }
        }

        Debug.Log(numberOfAudioFiles + " AUDIO FILES LOADED");

        foreach (Sound _sound in backGroundSound)
        {
            musicAudioSource.clip = _sound.AudioClip;
            Debug.Log("MUSIC" + _sound.AudioClip);
        }
        playNextBar();
       
        //Debug.Log("Audio Source "+musicAudioSource);

        //musicAudioSource.outputAudioMixerGroup = 

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerValue;
            playNextBar();
            musicAudioSource.Play();
        }
    }

    private void playNextBar()
    {
        Debug.Log("PLAY NEW");
        switch (myTagFlag)
        {
            case tagFlag.MODIFIED:
                soundsWhoMatchTheCurrentMood = new List<Sound>();
                foreach (Sound sound in backGroundSound)
                {
                    foreach (Sound._tag tag in sound.Tag)
                    {
                        if (tag == currentMood)
                        {
                            soundsWhoMatchTheCurrentMood.Add(sound);
                            break;
                        }
                    }
                }
                //soundsWhoMatchTheCurrentMood[0].AudioClip.
                break;
            case tagFlag.NOT_MODIFIED_YET:
                break;
            default:
                break;
        }
    }

    public Sound._tag Tag
    {
        get { return currentMood; }
        set
        {
            try
            {
                currentMood = (Sound._tag)value;
                myTagFlag = tagFlag.NOT_MODIFIED_YET;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }

    private enum tagFlag
    {
        MODIFIED,
        NOT_MODIFIED_YET
    }
}
