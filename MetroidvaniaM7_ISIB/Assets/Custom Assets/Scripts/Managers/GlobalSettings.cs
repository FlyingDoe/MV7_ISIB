using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global game manager. Manages the saving and loading of save data, keeps track of unlocked skill/teleporters/quests.
/// </summary>
public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance { get; private set; }

    private const string _gameDataFileName = "data.json";

    private SaveGame sG;

    private int skillNbr { get { return System.Enum.GetNames(typeof(SkillType)).Length; } }
    private int tprtrNbr { get { return System.Enum.GetNames(typeof(TprtrType)).Length; } }
    private int mapznNbr { get { return System.Enum.GetNames(typeof(MapznType)).Length; } }
    private int questNbr { get { return System.Enum.GetNames(typeof(QuestType)).Length; } }
    
    // =============================================================================

    #region UNITY FUNCTIONS

    private void Awake()
    {
        CheckInstance();
        CheckIfSaveDataExists();

        // DEBUGMODE
        UnlockEverything();
    }

    #endregion

    // =============================================================================

    #region AWAKE FUNCTIONS

    private void CheckInstance()
    {
        // If already exists, delete the new one.
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            // prevent destruction on scene change
            DontDestroyOnLoad(gameObject);
        }
    }
    private void CheckIfSaveDataExists()
    {
        // IF NO DATA, CREATE EMPTY FILE
        CreateEmptymySaveGame();
    }
    private void CreateEmptymySaveGame()
    {
        // Initialize dictionnaries
        sG = new SaveGame();
        sG.skills = new bool[skillNbr];
        sG.tprtrs = new bool[tprtrNbr];
        sG.mapzns = new bool[mapznNbr];
        sG.quests = new bool[questNbr];
    }

    #endregion

    // =============================================================================

    #region SAVEGAME FUNCTIONS

    public void Save()
    {
        FileManager.Save(_gameDataFileName, sG);
    }

    public void Load()
    {
        sG = FileManager.Load<SaveGame>(_gameDataFileName);
    }

    #endregion

    // =============================================================================
    #region GET INFOS FUNCTIONS

    /// <summary>
    /// Asks the GameManager if the player has unlocked a skill.
    /// </summary>
    /// <param name="skillType"></param>
    /// <returns></returns>
    public bool HasSkill(SkillType skillType)
    {
        return sG.skills[(int)skillType];
    }

    /// <summary>
    /// Asks the GameManager if the player has unlocked a teleporter.
    /// </summary>
    /// <param name="teleporterType"></param>
    /// <returns></returns>
    public bool HasTeleporter(TprtrType teleporterType)
    {
        return sG.tprtrs[(int)teleporterType];
    }

    /// <summary>
    /// Asks the GameManager if the player has visited a map.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public bool HasVisitedMap(MapznType map)
    {
        return sG.mapzns[(int)map];
    }

    /// <summary>
    /// Asks the GameManager if the player has finished a quest.
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public bool HasFinishedQuest(QuestType quest)
    {
        return sG.quests[(int)quest];
    }

    #endregion
    // =============================================================================

    #region UNLOCK FUNCTIONS

    public void UnlockSkill(SkillType skillType, bool shouldSave = true)
    {
        sG.skills[(int)skillType] = true;
        if (shouldSave)
            Save();
    }
    public void UnlockTeleporter(TprtrType tprtrType, bool shouldSave = true)
    {
        sG.tprtrs[(int)tprtrType] = true;
        if (shouldSave)
            Save();
    }
    public void UnlockMapZone(MapznType mapznType, bool shouldSave = true)
    {
        sG.mapzns[(int)mapznType] = true;
        if (shouldSave)
            Save();
    }
    public void FinishQuest(QuestType questType, bool shouldSave = true)
    {
        sG.quests[(int)questType] = true;
        if (shouldSave)
            Save();
    }

    public void LockEverything()
    {
        sG.skills = new bool[skillNbr];
        sG.tprtrs = new bool[tprtrNbr];
        sG.mapzns = new bool[mapznNbr];
        sG.quests = new bool[questNbr];
        Save();
    }
    public void UnlockEverything()
    {
        for (int i = 0; i < skillNbr; i++)
        {
            UnlockSkill((SkillType)i, false);
        }
        for (int i = 0; i < tprtrNbr; i++)
        {
            UnlockTeleporter((TprtrType)i, false);
        }
        for (int i = 0; i < mapznNbr; i++)
        {
            UnlockMapZone((MapznType)i, false);
        }
        for (int i = 0; i < questNbr; i++)
        {
            FinishQuest((QuestType)i, false);
        }
        Save();
    }
    #endregion

    // =============================================================================

    #region OTHER FUNCTIONS

    #endregion

}
