using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLisard : EnnemyBehavior
{
    //PlayerBehavior player;
    private bool hurt = false;
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
        MoveSpeed = 5.0f;
        JumpPower = 0.0f;
        RangeofLangue = 5f;
        TimeCoolDownAttacks = 2f;
        timeAttacks = 2f;
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
            if (timelanguetirer > TIMELTIRER) // l'idée c'était de renter la langue 
            {
                Debug.Log("on rendre la langue");
                langue.transform.localScale = new Vector3(0, 0, 0);
                GetComponentInChildren<Animator>().SetBool("atk", false);
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
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y , 0));
            this.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue )
        {
            //GetComponentInChildren<Animator>().SetBool("atk", false);
            GetComponentInChildren<Animator>().SetBool("running", true);
            timeAttacks = 0;
            transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));
        }else
            {
            //GetComponentInChildren<Animator>().SetBool("running", false);

            if (timeAttacks < 0)
            {
                if (tirerLangue)
                {
                    GetComponentInChildren<Animator>().SetBool("running", false);
                    GetComponentInChildren<Animator>().SetBool("atk", true);
                    langue.transform.localScale = new Vector3(1 , 0.01f , 0.02f); //tirer la langue 
                    Debug.Log("on tire la langue");
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
        GetComponentInChildren<Animator>().SetBool("running", false);
        GetComponentInChildren<Animator>().SetBool("hurt", true);
        hurt = false;
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
