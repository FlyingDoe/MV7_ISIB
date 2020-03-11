using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Ennemy")
        {
            other.GetComponent<EnnemyBehavior>().TakeAHit(CharacterBehavior.AtkType.bite);
        }
    }
}
