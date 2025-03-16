using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider), typeof(VisibleTag), typeof(SpaceBehavior))]
public class Space : MonoBehaviour
{
	public VisibleTag BurningTag { get; private set; }
	public SpaceBehavior Behavior { get; private set; }

	public void Awake()
	{
		BurningTag = GetComponent<VisibleTag>();
		Behavior = GetComponent<SpaceBehavior>();
	}

	/// <summary>
	/// Represents the space's biome.
	/// </summary>
	public BoardBiome Biome;

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
}
