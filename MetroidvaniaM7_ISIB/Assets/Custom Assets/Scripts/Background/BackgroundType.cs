using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a scriptable object can be changed from within the editor
// and is useful for holding data that won't be changed in-game
// mostly used for level design, I think
// like to switch between difficulty presets for example
// or graphical themes
[CreateAssetMenu(fileName = "BackgroundType", menuName = "ScriptableObjects/Background Type", order = 1)]
public class BackgroundType: ScriptableObject
{
    public BackgroundTypes Type;
    public RotationalAbility RotationalAbility;
    public List<Mesh> Meshes;
    public List<Material> Materials;
    public List<Material> NewMaterials;
    public Vector2 TextureTiling;
}

public enum BackgroundTypes
{
    WalkablePlatform,
    ObstaclePlatform,
    Wall,
    Decoration,
    Stalagmite
};

public enum RotationalAbility
{
    None,
    By180Degrees,
    By90Degrees,
    By30Degrees,
    Anything
}
