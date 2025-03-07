using UnityEngine;

/// <summary>
/// Abstract behavior of a space, responsible for deciding whatever space comes after it during a player's movement.
/// </summary>
public abstract class NextSpaceProvider : MonoBehaviour
{
	/// <summary>
	/// Provides the next space during a player's movement.
	/// </summary>
	public abstract Space NextSpace { get; }

	/// <summary>
	/// Provides all spaces this space could possibly lead into during a player's movement.
	/// </summary>
	public abstract Space[] NextSpaces { get; }
}