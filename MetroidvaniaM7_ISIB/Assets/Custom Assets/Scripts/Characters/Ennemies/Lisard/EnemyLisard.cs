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
        TimeCoolDownAttacks = 1f;
        timeAttacks = TimeCoolDownAttacks;
        Detectzone = 20f;
    }

    
    override
    public void MoveToPlayer()
    {
        
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, 0));
        this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue )
        {
            transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));
        }
        else
        {

            if (timeAttacks < 0)
            {
                if (tirerLangue)
                {
                    Debug.Log("on tire la langue");
                    langue.transform.localScale += new Vector3(RangeofLangue, 0, 0);
                    tirerLangue = false;                  
                }
                timeAttacks = TimeCoolDownAttacks;
            }
            else { timeAttacks -= Time.deltaTime;
                if (!tirerLangue)
                {
                    Debug.Log("on rendre la langue");
                    langue.transform.localScale -= new Vector3(RangeofLangue, 0, 0);
                    tirerLangue = true;
                }

            }
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
