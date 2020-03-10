﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    int maxHp;
    int hp;
    float moveSpeed;
    float jumpPower;
    enum AttType {slash, bite }

    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public int Hp { get => hp; protected set => hp = value; }
    public float MoveSpeed { get => moveSpeed; protected set => moveSpeed = value; }
    public float JumpPower { get => jumpPower; protected set => jumpPower = value; }
}
