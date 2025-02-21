using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PrototypeSpace.BoardRegion;

public class PrototypeManager : MonoBehaviour
{
	[SerializeField] private PrototypePlayer[] _players;
	[SerializeField] private Transform _board;

	private void Awake()
	{
		PrototypeSpace[] allSpaces = _board.GetComponentsInChildren<PrototypeSpace>(_board);
		foreach (PrototypeSpace space in allSpaces)
		{
			if (space.Type == PrototypeSpace.SpaceType.Negative && space.Region != Plains) _tornadoLandingSpaces.Add(space);

			if (space.Region is Plains or Mountains) _flammableSpaces.Add(space);
		}
	}

	#region Fire Logic
	[SerializeField] private int _initialFireSize = 3;
	[SerializeField] private int _fireDuration = 3;

	private bool _isFireOngoing;
	private int _roundsOfFireRemaining;

	private readonly List<PrototypeSpace> _flammableSpaces = new();
	private readonly Queue<PrototypeSpace> _spacesToExtinguish = new();
	private readonly Queue<PrototypeSpace> _spacesSetOnFire = new();

	/// <summary>
	/// Starts the fire disaster by starting fires at randomly-selected field and mountain tiles.
	/// </summary>
	[ContextMenu("Do Fire")]
	public void DoFireDisaster()
	{
		// Only one fire can be active at a time
		if (_isFireOngoing) return;
		_isFireOngoing = true;

		_roundsOfFireRemaining = _fireDuration - 1;

		// Randomly select the spaces at which the fire will start
		var fireStartingSpaces = _flammableSpaces.OrderBy(_ => Random.value).Take(_initialFireSize);

		// Set the selected spaces on fire
		foreach (PrototypeSpace space in fireStartingSpaces)
		{
			SetSpaceOnFire(space);
		}
	}

	/// <summary>
	/// Passes a round of fire, either spreading it or extinguishing it depending on the rounds remaining.
	/// </summary>
	[ContextMenu("Pass Round")]
	public void SimulateRoundPassing()
	{
		// If there is no fire, do nothing.
		if (!_isFireOngoing) return;

		if (_roundsOfFireRemaining > 0)
		{
			_roundsOfFireRemaining--;
			SpreadFire();
		}
		else
		{
			EndFireDisaster();
		}
	}

	/// <summary>
	/// Spreads fire from whichever spaces were set on fire during the previous round.
	/// </summary>
	private void SpreadFire()
	{
		int spacesToSpread = _spacesSetOnFire.Count;
		for (int i = 0; i < spacesToSpread; i++)
		{
			PrototypeSpace spaceToSpread = _spacesSetOnFire.Dequeue();

			foreach (PrototypeSpace space in spaceToSpread.AdjacentSpaces)
			{
				// Only spread fire to flammable spaces that aren't already on fire
				if (!space.IsOnFire && space.Region is Mountains or Plains) SetSpaceOnFire(space);
			}
		}
	}

	/// <summary>
	/// Ends the fire disaster by extinguishing all spaces set on fire.
	/// </summary>
	private void EndFireDisaster()
	{
		// Mark the fire as completed
		_isFireOngoing = false;

		// Set all the fire spaces to not on fire
		while (_spacesToExtinguish.Count > 0)
		{
			PrototypeSpace spaceToExtinguish = _spacesToExtinguish.Dequeue();
			spaceToExtinguish.IsOnFire = false;
		}

		_spacesSetOnFire.Clear();

		Debug.Log("Fire extinguished.");
	}

	/// <summary>
	/// Sets a space on fire and registers it as a newly-inflamed space.
	/// </summary>
	/// <param name="space">The space to set on fire.</param>
	private void SetSpaceOnFire(PrototypeSpace space)
	{
		Debug.Log($"{space.name} set on fire!");

		space.IsOnFire = true;

		_spacesSetOnFire.Enqueue(space);
		_spacesToExtinguish.Enqueue(space);
	}
	#endregion

	#region Tornado Logic
	/// <summary>
	/// List of all spaces a player could land on after a tornado (must be negative and non-plains)
	/// </summary>
	private readonly List<PrototypeSpace> _tornadoLandingSpaces = new();

	/// <summary>
	/// Blows all players occupying plains spaces onto random negative, non-plains spaces.
	/// </summary>
	[ContextMenu("Do Tornado")]
	public void DoTornadoDisaster()
	{
		int potentialLandingsCount = _tornadoLandingSpaces.Count;

		// Iterate through all players
		foreach (PrototypePlayer player in _players)
		{
			// Only blow them away if they're in the plains
			if (player.CurrentRegion != Plains) continue;

			// Select a random negative, non-plains space to plow the player
			int chosenLandingIndex = Random.Range(0, potentialLandingsCount);
			PrototypeSpace landingSpace = _tornadoLandingSpaces[chosenLandingIndex];

			// Move the player to the selected space
			player.transform.SetParent(landingSpace.transform, false);
		}
	}
	#endregion

	#region Tsunami Logic
	[ContextMenu("Do Tsunami")]
	public void DoTsunamiDisaster()
	{
		foreach (PrototypePlayer player in _players)
		{
			switch (player.CurrentRegion)
			{
				case Plains:
					break;

				case Mountains:
					player.IsFrozen = true;
					break;
			}
		}
	}
	#endregion
}
