using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_TEST : MonoBehaviour
{
    public bool changeTAG = false;

    AmbienMusicManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<AmbienMusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeTAG == true)
        {
            manager.Tag = Sound._tag.DISCOVERY;
            changeTAG = false;
            Debug.Log("CHANGE TAG" + changeTAG);
        }
        
    }
}
