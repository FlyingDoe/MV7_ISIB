using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance { get; private set; }

    private static string _gameDataFileName = "data.json";

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

}
