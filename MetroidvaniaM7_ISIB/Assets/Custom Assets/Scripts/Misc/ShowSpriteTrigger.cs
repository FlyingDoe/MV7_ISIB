using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpriteTrigger : MonoBehaviour
{
    SpriteRenderer rD;
    static Color transparentWhite = new Color(1, 1, 1, 0);
    float t;
    bool goingUp;
    bool goingDn;

    private void Awake()
    {
        rD = GetComponentInChildren<SpriteRenderer>();
        rD.color = transparentWhite;
        t = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            goingDn = false;
            goingUp = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            goingDn = true;
            goingUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 0 || t < 1)
        {
            rD.color = Color.Lerp(transparentWhite, Color.white, t);
            if (goingUp)
                t += Time.deltaTime *1.5f;
            if (goingDn)
                t -= Time.deltaTime * 1.5f;
            if (t <= 0 || t >= 1)
            {
                goingDn = false;
                goingUp = false;
            }
        }
    }
}
