using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveScriptDebug : MonoBehaviour
{

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += (Vector3.up * .1f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += (Vector3.left * .1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3.right * .1f);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3.down * .1f);
        }
    }
}
