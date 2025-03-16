using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider), typeof(VisibleTag), typeof(SpaceBehavior))]
public partial class Space : MonoBehaviour
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
	public Biome Biome;
}
