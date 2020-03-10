using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLisard : CharacterBehavior
{
    public Transform player;//PlayerBehavior player;
    public float RangeofLangue;
    public int dommage;
    public int intervalAttacks;
    public GameObject langue;

    void Start()
    {

        MaxHp = 5;
        Hp = MaxHp;
        MoveSpeed = 10.0f;
        JumpPower = 0.0f;
        RangeofLangue = 5f;
        dommage = 1;
        intervalAttacks = 1000; 
    }

    void Update()
    {
    }

    public void MoveToPlayer()
    {
        transform.LookAt(player.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        if (Vector3.Distance(transform.position, player.transform.position) > RangeofLangue)
        {
         transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0, 0));        
        }
        else
        {
            System.Threading.Thread.Sleep(intervalAttacks);
            langue.transform.localScale += new Vector3(RangeofLangue, 0, 0);      
        }
    }


    public void test()
        {
        if (langue.transform.localScale.x > RangeofLangue)
        {
            System.Threading.Thread.Sleep(200);
            langue.transform.localScale -= new Vector3(RangeofLangue, 0, 0);
        }
        }

    public void TakeAHit()
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
            Debug.Log("player doit prendre dégat");
        }
    }
}
