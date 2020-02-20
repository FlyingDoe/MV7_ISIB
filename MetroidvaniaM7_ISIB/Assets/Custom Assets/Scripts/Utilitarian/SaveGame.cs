using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Serializable objet used tu save the game data into a json file.
/// </summary>
[System.Serializable]
public class SaveGame
{
    [SerializeField]
    public bool[] skills;
    public bool[] tprtrs;
    public bool[] mapzns;
    public bool[] quests;
}

/// <summary>
/// Listes des différentes compétences exitant dans le jeu pour le joueur
/// </summary>
public enum SkillType
{
    jump,
    claw,
    bite
}
/// <summary>
/// Listes des différents téléporteurs dans le monde
/// </summary>
public enum TprtrType
{
    home
}
/// <summary>
/// Liste des différentes zones de cartes à afficher dans la minimap
/// </summary>
public enum MapznType
{
    home,
    tuto,
    cave1
}
/// <summary>
/// Liste des différentes quêtes exitant dans le jeu
/// </summary>
public enum QuestType
{
    mole,
    bat
}