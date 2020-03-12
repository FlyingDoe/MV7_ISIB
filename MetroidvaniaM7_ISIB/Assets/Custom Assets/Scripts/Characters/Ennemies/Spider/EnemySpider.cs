using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : EnnemyBehavior
{

    void Start()
    {

        MaxHp = 5;
        Hp = MaxHp;
        MoveSpeed = 10.0f;
        JumpPower = 0.0f;
        dommage = 1;
        intervalAttacks = 5f;
        Detectzone = 20f;
    }
    override
    public void doAllTime()
    { }
    override
    public void MoveToPlayer()
    {
        
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, 0));
        this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);

        if (Vector3.Distance(this.transform.position, player.transform.position) > 0)
        {
            //if (player.transform.position.x > this.transform.position.x)
                this.transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0)); 
        }
    }

    override
    public void TakeAHit(AtkType type)
    {
        Hp = Hp - 1;
        if (Hp == 0)
        {
            // annimation mort 
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Toucher");
        if (collider.tag == "Player")
        {
            if (player.state == "attack") {
                //TakeAHit();
            } else {
                Debug.Log("player doit prendre dégat");
            }
        }      
}}