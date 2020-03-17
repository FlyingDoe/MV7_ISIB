﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

// documentation on creating menu items: https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db
// documentation on creating buttons in editor scripts: https://learn.unity.com/tutorial/editor-scripting?language=en#5c7f8528edbc2a002053b5f8

public class BackgroundElementsManager
{
    public static BackgroundElement[] BgElements;

    public static Dictionary<BackgroundTypes, BackgroundType> BgParameters = new Dictionary<BackgroundTypes, BackgroundType>();

    //put this someplace else?
    public static string AlternativeMaterialsFolderName = "alternativeVersions";

    public static bool isDebugging = true;

    // to create a new type of background:
    // - create a new background type in the BackgroundTypes enum
    // - create a new scriptable object of type BackgroundType
    // (Assets > Create > Scriptable Object > Background Type)
    // and fill it out appropriately
    // - create a prefab with ( TODO: the rest of the explanations)

    [MenuItem("CustomScripts/BackgroundElements/InitializeManager", false, 0)]
    public static void InitializeManager()
    {
        BackgroundType[] types = Resources.FindObjectsOfTypeAll<BackgroundType>();
        if (isDebugging)
        {
            Debug.Log("types count: " + types.Length);
            Debug.Log("bgParams length: " + BgParameters.Count);
        }
        foreach (BackgroundType type in types)
        {
            if (!BgParameters.ContainsValue(type))
            {
                if(isDebugging)
                {
                    Debug.Log("adding type " + type.name);
                }
                BgParameters.Add(type.Type, type);
            }
        }

        // create x versions of each material per type, ensure it has the right scale
        // and offset it x different ways
        // we need to use Database.CreateAsset so there is a path on disk to the new materials;
        // otherwise the ScriptableObject trying to link to the new materials throws an error
        Vector2 tiling;
        Material newMat;
        string matPath;
        foreach(BackgroundType type in BgParameters.Values)
        {
            // we find the folder containing the materials
            // and create a subfolder for the variations
            matPath = AssetDatabase.GetAssetPath(type.Materials[0]);
            // TODO: put this someplace else: the correct separator is everyone's business
            char correctSeparator = matPath.LastIndexOf(Path.DirectorySeparatorChar) > 0 ? Path.DirectorySeparatorChar : Path.AltDirectorySeparatorChar;
            int lastSlash = matPath.LastIndexOf(correctSeparator);
            matPath = matPath.Remove(lastSlash);
            if(!AssetDatabase.IsValidFolder(matPath + correctSeparator + AlternativeMaterialsFolderName))
            {
                AssetDatabase.CreateFolder(matPath, AlternativeMaterialsFolderName);
                if (isDebugging)
                {
                    Debug.Log("creating folder at " + matPath + correctSeparator + AlternativeMaterialsFolderName);
                }
            }

            if (type.NewMaterials == null || type.NewMaterials.Count == 0)

            {
                tiling = type.TextureTiling;
                foreach (Material mat in type.Materials)
                {
                    mat.mainTextureScale = tiling;
                    for (int i = 0; i < 3; i++)
                    {
                        newMat = new Material(mat)
                        {
                            mainTextureOffset = new Vector2(Random.Range((float)0, (float)1), Random.Range((float)0, (float)1))
                        };
                        AssetDatabase.CreateAsset(newMat, matPath + correctSeparator + AlternativeMaterialsFolderName + correctSeparator + (mat.name + "_" + type.name + "_v" + i + ".mat"));
                        type.NewMaterials.Add(newMat);
                        if (isDebugging)
                        {
                            Debug.Log("added material " + newMat + " to the list.");
                        }
                    }
                }
                if (isDebugging)
                {
                    Debug.Log("Just treated " + type.name + " background type.");
                }
            }
        }
    }

    /// <summary> 
    /// get a list of all background elements in the scene
    /// and initialize them
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/InitializeElements", false, 1)]
    public static void InitializeAllElements()
    {
        BgElements = Resources.FindObjectsOfTypeAll<BackgroundElement>();
        foreach (BackgroundElement element in BgElements)
        {
            element.Initialize();

            if(element.GetBackgroundType() == BackgroundTypes.ObstaclePlatform)
            {
                element.SetUnwalkableCollider();
            }
            else
            {
                element.DeactivateUnwalkableCollider();
            }
        }
    }

    /// <summary>
    /// randomize background elements' pretty look
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/RandomizeBackground", false, 2)]
    public static void RandomizeBackground()
    {
        BackgroundTypes currentType;
        int meshNumber;
        int matNumber;
        foreach (BackgroundElement element in BgElements)
        {
            // start showing pretty background elements
            element.ActivatePrettyBackground();

            if (element.IsAspectLocked)
            {
                continue;
            }

            currentType = element.GetBackgroundType();
            // choose random mesh
            meshNumber = Random.Range(0, BgParameters[currentType].Meshes.Count);
            element.ChangeMesh(BgParameters[currentType].Meshes[meshNumber]);
            // choose random material
            matNumber = Random.Range(0, BgParameters[currentType].NewMaterials.Count);
            element.ChangeMat(BgParameters[currentType].NewMaterials[matNumber]);

            if (element.IsRotationLocked)
            {
                continue;
            }
            // if allowed, choose random rotation
            switch (BgParameters[currentType].RotationalAbility)
            {
                case RotationalAbility.None:
                    break;
                case RotationalAbility.By180Degrees:
                    element.ChangeRotation(RandomRotationValue(180));
                    break;
                case RotationalAbility.By90Degrees:
                    element.ChangeRotation(RandomRotationValue(90));
                    break;
                case RotationalAbility.By30Degrees:
                    element.ChangeRotation(RandomRotationValue(30));
                    break;
                case RotationalAbility.Anything:
                    element.ChangeRotation(RandomRotationValue(1));
                    break;
            }
        }
    }

    /// <summary>
    /// reset meshes to simple geometric ones
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/ResetBackground", false, 98)]
    public static void ResetElements()
    {
        foreach (BackgroundElement element in BgElements)
        {
            element.Reset();
        }
    }

    /// <summary>
    /// reset manager's types list and each type info to default empty state
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/ResetManager", false, 99)]
    public static void ResetManager()
    {
        foreach (BackgroundType type in BgParameters.Values)
        {
            type.NewMaterials.Clear();
            if (isDebugging)
            {
                Debug.Log("reset " + type.name + ": done!");
            }
        }
        BgParameters.Clear();
    }

    // TODO: add a way to delete the alternative versions of the materials using AssetDatabase.DeleteAsset
    // but add a confirmation window before executing it

    // TODO: everything
    public static void BackgroundElementsInfo()
    {
        // has manager been initialized? when?
        // if so, which types does it know?
        // have elements been initialized? when? how many?
        // have elements been randomized? When? how many?
        // number of geometric elements?
        // number of pretty elements?
    }

    /// <summary>
    /// returns a random rotation taken from each -step- degrees; 
    /// if it can only turn 90 degrees, il will return either 
    /// 0, 90, 180 or 240 degrees
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public static int RandomRotationValue(int step)
    {
        return Random.Range(0, (360 / step)) * step;
    }
}