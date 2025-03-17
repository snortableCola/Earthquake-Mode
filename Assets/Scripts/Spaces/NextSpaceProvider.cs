using UnityEngine;

public class NextSpaceProvider : MonoBehaviour
{
	[SerializeField] private Space[] _nextSpaces;

	/// <summary>
	/// Provides all spaces this space could possibly lead into during a player's movement.
	/// </summary>
	public Space[] NextSpaces => _nextSpaces;
}