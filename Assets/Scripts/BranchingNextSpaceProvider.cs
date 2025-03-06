using UnityEngine;

/// <summary>
/// Behavior for a space that leads directly into multiple separate spaces, one of which must be selected during player movement.
/// </summary>
public class BranchingNextSpaceProvider : NextSpaceProvider
{
	/// <summary>
	/// An array of spaces that may be moved to after this one during player movement, defined in the editor.
	/// </summary>
	[SerializeField] private Space[] _nextSpaces;

	// Currently, in these branching situations, a random space is selected for the player.
	// We'll have to implement player choice later.
	public override Space NextSpace => _nextSpaces[Random.Range(0, _nextSpaces.Length)];

	public override Space[] NextSpaces => _nextSpaces;
}
