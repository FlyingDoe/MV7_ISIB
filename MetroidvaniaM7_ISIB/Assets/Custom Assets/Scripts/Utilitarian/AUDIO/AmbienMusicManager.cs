using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AmbienMusicManager : MonoBehaviour
{
    public double BPM = 100;
    public AudioMixer mixer;

    private double beatLength;
    private float timer;
    private float timerValue;

    private Sound._tag currentMood = Sound._tag.CALM;
    private tagFlag myTagFlag = tagFlag.MODIFIED;

    private List<AudioSource> musicAudioSources = new List<AudioSource>();
    


    private int numberOfAudioFiles = 0;

    private List<Sound> backGroundSound = new List<Sound>();
    private List<Sound> soundsWhoMatchTheCurrentMood = new List<Sound>();

    private int audioSourceIndex;
    private int audioSourcePlayIndex;

    System.Random rand = new System.Random();

    void Awake()
    {
        //SETTING MUSIC BAR LENGHT
        double beatLength = 60.0 / BPM;
        timerValue = (float)(beatLength * 32);
        timer = 0;

        //Getting the mixer 
        mixer = Resources.Load<AudioMixer>("mixer");

        //CREATING THE AUDIO SOURCE 
        for (int i = 0; i < 2; i++)
        {
            //Debug.Log("CAMERA");
            musicAudioSources.Add(GameObject.FindObjectOfType<Camera>().gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
            
        }
        musicAudioSources[0].outputAudioMixerGroup = mixer.FindMatchingGroups("BACKGROUND MUSIC")[0];

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
            musicAudioSources[audioSourceIndex%2].clip = _sound.AudioClip;
            audioSourceIndex++;
            //Debug.Log("MUSIC" + _sound.AudioClip);
        }
       
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
            playNextBar(audioSourcePlayIndex % 2);
            musicAudioSources[audioSourcePlayIndex%2].Play();
            //Debug.Log(audioSourcePlayIndex % 2);
            audioSourcePlayIndex++;
        }
        //Debug.Log("TAG " + currentMood + " " + myTagFlag);
    }

    private void playNextBar(int i) //i = audioSourceIndex
    {
        //Debug.Log("PLAY NEW");
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
                myTagFlag = tagFlag.NOT_MODIFIED_YET;
                //soundsWhoMatchTheCurrentMood[0].AudioClip.
                break;
            case tagFlag.NOT_MODIFIED_YET:
                break;
            default:
                break;
        }
        if (soundsWhoMatchTheCurrentMood.Count>0)
        {
            int index = rand.Next(0, soundsWhoMatchTheCurrentMood.Count);

            musicAudioSources[i].clip = soundsWhoMatchTheCurrentMood[index].AudioClip;


        }
        else
        {
            int index = rand.Next(0, backGroundSound.Count);
            musicAudioSources[i].clip = backGroundSound[index].AudioClip;
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
                myTagFlag = tagFlag.MODIFIED;

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
