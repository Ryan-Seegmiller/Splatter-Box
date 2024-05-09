using UnityEngine;

/// <summary>
/// scriptable object that represents a type of disaster
/// </summary>
[CreateAssetMenu]
public class DisasterInfo : ScriptableObject
{
//-----------------------------------------------------------------------------
// readonly members
//-----------------------------------------------------------------------------


    /// <summary>
    /// the prefab to spawn for this disaster
    /// </summary>
    public GameObject disasterPrefab;

    /// <summary>
    /// the name of this disaster
    /// </summary>
    public string disasterName;

    /// <summary>
    /// the icon that represents this disaster
    /// </summary>
    public Texture disasterIcon;


//-----------------------------------------------------------------------------
}