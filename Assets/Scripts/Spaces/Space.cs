using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider), typeof(VisibleTag))]
public class Space : MonoBehaviour
{
    [HideInInspector] public VisibleTag BurningTag;

	public void Awake()
	{
		BurningTag = GetComponent<VisibleTag>();
	}

    /// <summary>
    /// Represents the space's biome.
    /// </summary>
    public BoardBiome Biome;

    /// <summary>
    /// Represents the space's typical behavior when landed on by a player.
    /// </summary>
    public SpaceType Type;

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
        Oil,
		Resource,
		Negative,
        Transport,
        Bonus,
        Shop,
        Sabotage
    }
}
