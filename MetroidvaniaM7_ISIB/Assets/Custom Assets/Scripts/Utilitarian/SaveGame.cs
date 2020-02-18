using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveGame
{
    [SerializeField]
    public bool[] skills;
    public bool[] tprtrs;
    public bool[] mapzns;
    public bool[] quests;
}


public enum SkillType
{
    jump,
    claw,
    bite
}
public enum TprtrType
{
    home
}
public enum MapznType
{
    home,
    tuto,
    cave1
}
public enum QuestType
{
    mole,
    bat
}