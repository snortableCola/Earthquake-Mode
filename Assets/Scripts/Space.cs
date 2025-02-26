using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider))]
public class Space : MonoBehaviour
{
    /// <summary>
    /// Represents whether or not the space is on fire.
    /// </summary>
    public bool IsOnFire;

    /// <summary>
    /// Represents the space's biome.
    /// </summary>
    public BoardBiome Biome;

    /// <summary>
    /// Represents the space's typical behavior when landed on by a player.
    /// </summary>
    public SpaceType Type;

    /// <summary>
    /// An array of spaces that are considered directly adjacent to this space.
    /// </summary>
    public Space[] AdjacentSpaces;

    /// <summary>
    /// An enum containing all potential biomes a space could occupy.
    /// </summary>
    public enum BoardBiome
    {
        None,
        Coast,
        Plains,
        Mountains
    }

    /// <summary>
    /// An enum containing descriptors for each way a space could behave when landed on by a player.
    /// </summary>
    public enum SpaceType
    {
        None,
        Negative
        // Obviously, there will be more in the final product, this is simply all that was needed for disaster functionality
    }
}
