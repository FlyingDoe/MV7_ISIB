using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
  [SerializeField] CharacterBehavior.AtkType atktype;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Ennemy")
        {
            other.GetComponent<EnnemyBehavior>().TakeAHit(atktype);
        }
        if (atktype == CharacterBehavior.AtkType.FireBall && other.tag !="Player")
        {
            Debug.Log(other.name);
            this.gameObject.SetActive(false);
        }
    }
}
