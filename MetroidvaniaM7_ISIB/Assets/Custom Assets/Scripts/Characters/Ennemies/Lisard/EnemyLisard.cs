using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLisard : EnnemyBehavior
{   
    //PlayerBehavior player;
    public float RangeofLangue;
    private float timeAttacks;
    private float TimeCoolDownAttacks;
    private bool tirerLangue = true;
    public GameObject langue;

    void Start()
    {
        timeAttacks = 0;
        MaxHp = 5;
        Hp = MaxHp;
        MoveSpeed = 10.0f;
        JumpPower = 0.0f;
        RangeofLangue = 5f;
        TimeCoolDownAttacks = 0.3f;
        Detectzone = 20f;
    }

    override
    public void MoveToPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, 0));
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue)
        {
         transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));        
        }
        else
        {
            //Debug.Log("on veut attacker 1");
            if (timeAttacks > TimeCoolDownAttacks)
            {
                //Debug.Log("on veut attacker 2");

                if (tirerLangue)
                {
                    langue.transform.localScale += new Vector3(RangeofLangue, 0, 0);
                    tirerLangue = false;
                    timeAttacks = 0;
                }  
            }
            else { timeAttacks += Time.deltaTime; }

            
        }
    }

    override
    public void TakeAHit(AtkType type)
    {
        Hp = Hp - 1;

        if (Hp <= 0)
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
            Debug.Log("player doit prendre dégat");
        }
    }
}
