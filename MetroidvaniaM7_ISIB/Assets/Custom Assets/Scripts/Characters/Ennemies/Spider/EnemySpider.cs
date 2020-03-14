using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : EnnemyBehavior
{
    private bool attack = false;
    private bool hurt = false;
    private float timer = 2f;
    void Start()
    {

        MaxHp = 1;
        Hp = MaxHp;
        MoveSpeed = 5.0f;
        JumpPower = 0.0f;
        dommage = 1;
        intervalAttacks = 5f;
        Detectzone = 20f;
    }
    override
    public void doAllTime()
    {
        
        if (hurt == true)
        {
            
            if (timer < 0)
            {
                if (GetComponentInChildren<Animator>().GetBool("hurt") == false) 
                hurt = false;
                timer = 2f;
            }
        
            else
            {
                timer -= Time.deltaTime;
            }

        }
        else if (attack == true)
        { 
            if (timer < 0)
            {
                GetComponentInChildren<Animator>().SetBool("attack", false);
                attack = false;
                timer = 2f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else if (Vector3.Distance(this.transform.position, player.transform.position) < Detectzone)
        {
            GetComponentInChildren<Animator>().SetBool("running", true);
        }
        else
            GetComponentInChildren<Animator>().SetBool("running", false);

    }



    override
    public void MoveToPlayer()
    {

        if (this.transform.position.y - player.transform.position.y < 0.5 && this.transform.position.y - player.transform.position.y > -0.5)
        {
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, 0));
            this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
            this.transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));
        }
    }

    override
    public void TakeAHit(AtkType type)
    {
        if (type == AtkType.bite)
        {
            Hp = Hp - 1;
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("hurt", true);
            hurt = true;
        }
        if (type == AtkType.slash)
        {
            Hp = Hp - 1;
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("hurt", true);
            hurt = true;
        }

        if (Hp == 0)
        {
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        
        //Debug.Log("Toucher12");
        if (collider.tag == "Player")
        {
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("attack", true);
            attack = true;
            

        }
        }
    }