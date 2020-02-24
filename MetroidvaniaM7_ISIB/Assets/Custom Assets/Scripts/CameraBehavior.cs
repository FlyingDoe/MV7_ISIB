using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    moveScriptDebug playerMove;

    public Vector3 lastDirTarget = Vector3.zero;
    public Vector3 currentDir = Vector3.zero;
    Vector3 vDir;
    Vector3 vPos;
    public float smoothTimeDir = 0.1F;
    public float smoothTimePos = 0.3F;

    private void Awake()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<moveScriptDebug>();
    }

    private void Update()
    {
        if (playerMove)
        {
            currentDir = Vector3.SmoothDamp(currentDir, lastDirTarget, ref vDir, smoothTimeDir);
            transform.position = Vector3.SmoothDamp(transform.position, playerMove.transform.position + currentDir, ref vPos, smoothTimePos);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            lastDirTarget = Vector3.up * 1.5f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            lastDirTarget = Vector3.left * 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lastDirTarget = Vector3.right * 2;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lastDirTarget = Vector3.down;
        }
    }
}
