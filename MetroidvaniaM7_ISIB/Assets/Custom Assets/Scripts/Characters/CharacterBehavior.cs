using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    int maxHp;
    int hp;
    int attackPoint;
    float moveSpeed;
    float jumpPower;

    public int AttackPoint { get => attackPoint; protected set => attackPoint = value; }
    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public int Hp { get => hp; protected set => hp = value; }
    public float MoveSpeed { get => moveSpeed; protected set => moveSpeed = value; }
    public float JumpPower { get => jumpPower; protected set => jumpPower = value; }
}
