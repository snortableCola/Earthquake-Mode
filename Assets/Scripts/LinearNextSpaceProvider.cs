using UnityEngine;

/// <summary>
/// Behavior for a space that leads directly into another space during player movement.
/// </summary>
public class LinearNextSpaceProvider : NextSpaceProvider
{
	/// <summary>
	/// The single space after this one for player movement, defined in the editor.
	/// </summary>
	[SerializeField] private Space _nextSpace;

	public override Space NextSpace => _nextSpace;
}