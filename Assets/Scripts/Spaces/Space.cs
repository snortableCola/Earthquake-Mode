using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider), typeof(VisibleTag), typeof(SpaceBehavior))]
public partial class Space : MonoBehaviour
{
	public VisibleTag BurningTag { get; private set; }
	public VisibleTag HighlightTag { get; private set; }
	public SpaceBehavior Behavior { get; private set; }

	public void Awake()
	{
		VisibleTag[] visibleTags = GetComponents<VisibleTag>();
		BurningTag = visibleTags[0];
		HighlightTag = visibleTags[1];
		Behavior = GetComponent<SpaceBehavior>();
	}

	/// <summary>
	/// Represents the space's biome.
	/// </summary>
	public Biome Biome;
}
