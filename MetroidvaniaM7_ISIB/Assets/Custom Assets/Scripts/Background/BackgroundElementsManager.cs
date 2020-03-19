﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

// documentation on creating menu items: https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db
// documentation on creating buttons in editor scripts: https://learn.unity.com/tutorial/editor-scripting?language=en#5c7f8528edbc2a002053b5f8

public static class BackgroundElementsManager
{
    public static BackgroundElement[] BgElements;

    public static Dictionary<BackgroundTypes, BackgroundType> BgParameters = new Dictionary<BackgroundTypes, BackgroundType>();

    //put this someplace else?
    public static string AlternativeMaterialsFolderName = "alternativeVersions";
    public static List<string> AlternateMaterialsFoldersPaths = new List<string>();
    public static char CorrectSeparator;

    public static bool isDebugging = false;

    // to create a new type of background:
    // - create a new background type in the BackgroundTypes enum
    // - create a new scriptable object of type BackgroundType
    // (Assets > Create > Scriptable Object > Background Type)
    // and fill it out appropriately
    // - create a prefab composed of an Empty with the BackgroundElement script, 
    // one BoxCollider and a Rigidbody if it should block out the space above it, 
    // and with as children:
    //// - one placeholder tagged EditorOnly containing:
    ////// - a Mesh Filter of a simple geometric shape
    ////// - a Mesh Renderer
    ////// - a Collider (if canCollide)
    ////// - a Rigidbody (if Collider)
    //// - and one pretty-looking object containing:
    ////// - a Mesh Filter of (one of) the final Mesh
    ////// - a Mesh Renderer
    ////// - a Collider (either an automatic convex mesh collider or a geometric one)
    ////// (if canCollide)
    ////// - a Rigidbody (if Collider)

    [MenuItem("CustomScripts/BackgroundElements/InitializeAll   ", false, 0)]
    public static void InitializeAll()
    {
        InitializeManager();
        FindAllElements();
        InitializeAllElements();
    }


    [MenuItem("CustomScripts/BackgroundElements/More/InitializeManager", false, 200)]
    public static void InitializeManager()
    {
        BackgroundType[] types = Resources.FindObjectsOfTypeAll<BackgroundType>();
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
        string matFolder;
        foreach(BackgroundType type in BgParameters.Values)
        {
            // we find the folder containing the materials
            // and create a subfolder for the variations
            matPath = AssetDatabase.GetAssetPath(type.Materials[0]);
            // TODO: put this someplace else: the correct separator is everyone's business
            CorrectSeparator = matPath.LastIndexOf(Path.DirectorySeparatorChar) > 0 ? Path.DirectorySeparatorChar : Path.AltDirectorySeparatorChar;
            int lastSlash = matPath.LastIndexOf(CorrectSeparator);
            matPath = matPath.Remove(lastSlash);
            matFolder = matPath + CorrectSeparator + AlternativeMaterialsFolderName;
            if (!AssetDatabase.IsValidFolder(matFolder))
            {
                AssetDatabase.CreateFolder(matPath, AlternativeMaterialsFolderName);

                if (isDebugging)
                {
                    Debug.Log("creating folder at " + matFolder);
                }
            }
            // for cleaning up purposes later
            if (!AlternateMaterialsFoldersPaths.Contains(matFolder))
            {
                Debug.Log("adding path " + matFolder);
                AlternateMaterialsFoldersPaths.Add(matFolder);
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
                        AssetDatabase.CreateAsset(newMat, matPath + CorrectSeparator + AlternativeMaterialsFolderName + CorrectSeparator + (mat.name + "_" + type.name + "_v" + i + ".mat"));
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

    [MenuItem("CustomScripts/BackgroundElements/More/Find All Elements", false, 200)]
    public static void FindAllElements()
    {
        BgElements = Resources.FindObjectsOfTypeAll<BackgroundElement>();
    }

    /// <summary> 
    /// get a list of all background elements in the scene
    /// and initialize them
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/More/Initialize Elements", false, 200)]
    public static void InitializeAllElements()
    {
        foreach (BackgroundElement element in BgElements)
        {
            element.Initialize(verbose: false);

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
    [MenuItem("CustomScripts/BackgroundElements/RandomizeBackground", false, 50)]
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

            // choose random scale
            if (BgParameters[currentType].Scalability != 0)
            {
                element.ChangeScale(Random.Range(-BgParameters[currentType].Scalability/100, BgParameters[currentType].Scalability/100));
            }
        }
    }


    /// <summary>
    /// switch between geometric and pretty views
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/Switch view", false, 50)]
    public static void SwitchView()
    {
        foreach (BackgroundElement element in BgElements)
        {
            element.ActivatePrettyBackground(!element.CheckPrettyState());
        }
    }


    /// <summary>
    /// reset meshes to simple geometric ones
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/More/Reset Background", false, 200)]
    public static void ResetElements()
    {
        foreach (BackgroundElement element in BgElements)
        {
            element.Reset();
        }
    }

    [MenuItem("CustomScripts/BackgroundElements/More/Remove Alternate Materials", false, 200)]
    public static void RemoveAlternateMaterials()
    {
        foreach(string path in AlternateMaterialsFoldersPaths)
        {
            Directory.Delete(path, true);
        }
        AssetDatabase.Refresh();
        AlternateMaterialsFoldersPaths.Clear();
        foreach (BackgroundType type in BgParameters.Values)
        {
            type.NewMaterials.Clear();
        }
    }

    /// <summary>
    /// reset manager's types list and each type info to default empty state
    /// </summary>
    [MenuItem("CustomScripts/BackgroundElements/More/ResetManager", false, 200)]
    public static void ResetManager()
    {
        //TODO: add a warning that ResetElems should be done before this one
        // because it loses the connections they need
        BgParameters.Clear();
    }

    [MenuItem("CustomScripts/BackgroundElements/Reset All", false, 50)]
    public static void ResetAll()
    {
        RemoveAlternateMaterials();
        ResetElements();
        ResetManager();
    }

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

    // TODO: progress bar

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