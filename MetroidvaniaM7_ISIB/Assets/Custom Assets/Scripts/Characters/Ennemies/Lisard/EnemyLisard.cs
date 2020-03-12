using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLisard : EnnemyBehavior
{   
    //PlayerBehavior player;
    public float RangeofLangue;
    public float timeAttacks;
    public float TimeCoolDownAttacks;
    private float timelanguetirer;
    private float TIMELTIRER;
    private bool tirerLangue = true;

    public GameObject langue;

    void Start()
    {
        TIMELTIRER = 1f;
        timeAttacks = 0;
        MaxHp = 5;
        Hp = MaxHp;
        MoveSpeed = 10.0f;
        JumpPower = 0.0f;
        RangeofLangue = 5f;
        TimeCoolDownAttacks = 2f;
        timeAttacks = TimeCoolDownAttacks;
        Detectzone = 20f;
    }
    override
    public void doAllTime() {
    if (!tirerLangue)
        {
            if (timelanguetirer > TIMELTIRER)
            {
                Debug.Log("on rendre la langue");
                langue.transform.localScale -= new Vector3(RangeofLangue, 0, 0);
    tirerLangue = true;
                timelanguetirer = 0;
            }
            else { timelanguetirer += Time.deltaTime; }
        }
    }

    override
    public void MoveToPlayer()
    {
        
        if (tirerLangue)
        {
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, 0));
            this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue )
        {
            timeAttacks = 0;
            transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));
        }else
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
