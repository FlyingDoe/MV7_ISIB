using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    int maxHp;
    int hp;

    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public int Hp { get => hp; protected set => hp = value; }
}
