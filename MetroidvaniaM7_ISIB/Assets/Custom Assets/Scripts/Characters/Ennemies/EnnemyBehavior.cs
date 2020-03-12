using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehavior : CharacterBehavior
{
    public PlayerBehavior player;
    public int dommage;
    public float intervalAttacks;
    public float Detectzone = 0f;

    void Start()
    {
    }
    void FixedUpdate()
    {
        this.doAllTime();
        if (Vector3.Distance(this.transform.position, player.transform.position) < Detectzone)
        {
            this.MoveToPlayer();
        }


    }
    public virtual void doAllTime() { }
    public virtual void MoveToPlayer() { }
    
    public virtual void TakeAHit(AtkType type)
    {

    }   
}