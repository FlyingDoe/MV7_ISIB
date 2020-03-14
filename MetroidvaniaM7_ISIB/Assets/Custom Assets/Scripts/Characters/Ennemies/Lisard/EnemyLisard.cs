using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLisard : EnnemyBehavior
{
    //PlayerBehavior player;
    private bool hurt = false;

    public float RangeofLangue = 5f;
    //private float timeAttacks;
    //public float TimeCoolDownAttacks;
    private bool tirerLangue = true;
   // public GameObject langue;

    void Start()
    {

        //timeAttacks = 0;
        MaxHp = 2;
        Hp = MaxHp;
        MoveSpeed = 5.0f;
        JumpPower = 0.0f;
         //longeur de la langue dans l'anim
        //TimeCoolDownAttacks = 3f; // vue que l'animation est lente j'ai juste fais que l'ataque sois le temps du cool down 
        //timeAttacks = TimeCoolDownAttacks;
        Detectzone = 20f;
    }
    override
    public void doAllTime() {

        if (hurt == true)
        {
            float timer = 2f;
            if (timer < 0)
            {
                GetComponentInChildren<Animator>().SetBool("hurt", false);
                hurt = false;
                timer = 2f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
            if (!tirerLangue)
        {
            Debug.Log("on rendre la langue");
            GetComponentInChildren<Animator>().SetBool("atk", false);
             
            //timeAttacks = TimeCoolDownAttacks;
            tirerLangue = true;
        }
    }

    override
    public void MoveToPlayer()
    {
        
        if (tirerLangue)
        {
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y , 0));
            this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue )
        {
            //GetComponentInChildren<Animator>().SetBool("atk", false);
            
            tirerLangue = false;
            GetComponentInChildren<Animator>().SetBool("running", true);

            transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));
        }else
            {
            //GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("atk", true);
            /*if (timeAttacks < 0)
            {
                if (tirerLangue)
                {*/
                    tirerLangue =false;
            /*
                }
                timeAttacks = TimeCoolDownAttacks;
            }
            else { timeAttacks -= Time.deltaTime;
               
            }*/
        }
    }

    override
    public void TakeAHit(AtkType type)
    {
        if (type == AtkType.bite) {
            Hp = Hp - 1;
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("hurt", true);
            hurt = false;
        }
        if (type == AtkType.slash)
        {
            Hp = Hp - 1;
            GetComponentInChildren<Animator>().SetBool("running", false);
            GetComponentInChildren<Animator>().SetBool("hurt", true);
            hurt = false;
        }
        // GetComponentInChildren<Animator>().SetBool("hurt", false); à la fin animation 
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
