using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Space.BoardBiome;

public class DisasterManager : MonoBehaviour
{
	[SerializeField] private Player[] _players;
	[SerializeField] private Transform _board;

	private Dictionary<Space, List<Space>> _adjacencies;

	private void Awake()
	{
		Space[] allSpaces = _board.GetComponentsInChildren<Space>(_board);

		_adjacencies = allSpaces.ToDictionary(space => space, _ => new List<Space>());

		foreach (Space space in allSpaces)
		{
			Space[] nextSpaces = space.GetComponent<NextSpaceProvider>().NextSpaces;

			foreach (Space nextSpace in nextSpaces)
			{
				_adjacencies[nextSpace].Add(space);
				_adjacencies[space].Add(nextSpace);
			}

			if (space.Type == Space.SpaceType.Negative && space.Biome != Plains) _tornadoLandingSpaces.Add(space);

			if (space.Biome is Plains or Mountains) _flammableSpaces.Add(space);
		}
	}

	#region Fire Logic
	[SerializeField] private int _initialFireSize = 3;
	[SerializeField] private int _fireDuration = 3;

	private bool _isFireOngoing;
	private int _roundsOfFireRemaining;

	private readonly List<Space> _flammableSpaces = new();
	private readonly Queue<Space> _spacesToExtinguish = new();
	private readonly Queue<Space> _spacesSetOnFire = new();

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
		foreach (Space space in fireStartingSpaces)
		{
			SetSpaceOnFire(space);
		}
	}

	/// <summary>
	/// Passes a round of fire, either spreading it or extinguishing it depending on the rounds remaining.
	/// </summary>
	[ContextMenu("Pass Round")]
	public void PassFireRound()
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
			Space spaceToSpread = _spacesSetOnFire.Dequeue();

			foreach (Space space in _adjacencies[spaceToSpread])
			{
				// Only spread fire to flammable spaces that aren't already on fire
				if (!space.IsOnFire && space.Biome is Mountains or Plains) SetSpaceOnFire(space);
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
			Space spaceToExtinguish = _spacesToExtinguish.Dequeue();
			spaceToExtinguish.IsOnFire = false;
		}

		_spacesSetOnFire.Clear();

		Debug.Log("Fire extinguished.");
	}

	/// <summary>
	/// Sets a space on fire and registers it as a newly-inflamed space.
	/// </summary>
	/// <param name="space">The space to set on fire.</param>
	private void SetSpaceOnFire(Space space)
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
	private readonly List<Space> _tornadoLandingSpaces = new();

	/// <summary>
	/// Blows all players occupying plains spaces onto random negative, non-plains spaces.
	/// </summary>
	[ContextMenu("Do Tornado")]
	public void DoTornadoDisaster()
	{
		int potentialLandingsCount = _tornadoLandingSpaces.Count;

		// Iterate through all players
		foreach (Player player in _players)
		{
			// Only blow them away if they're in the plains
			if (player.CurrentRegion != Plains) continue;

			// Select a random negative, non-plains space to plow the player
			int chosenLandingIndex = Random.Range(0, potentialLandingsCount);
			Space landingSpace = _tornadoLandingSpaces[chosenLandingIndex];

			// Move the player to the selected space
			player.MoveTo(landingSpace.transform);
		}
	}
	#endregion

	#region Tsunami Logic
	[ContextMenu("Do Tsunami")]
	public void DoTsunamiDisaster()
	{
		foreach (Player player in _players)
		{
			switch (player.CurrentRegion)
			{
				case Coast:
					TsunamiPush(player);
					break;

				case Mountains:
					player.IsFrozen = true;
					break;
			}
		}
	}

	private void TsunamiPush(Player player)
	{
		Space startingSpace = player.GetComponentInParent<Space>();

		HashSet<Space> coastSpaces = new() { startingSpace };
		Queue<Space> spacesToCheck = new(_adjacencies[startingSpace]);

		List<Space> potentialDestinations = new();
		while (potentialDestinations.Count == 0)
		{
			int checks = spacesToCheck.Count;
			for (int i = 0; i < checks; i++)
			{
				Space space = spacesToCheck.Dequeue();

				if (space.Biome is not Coast)
				{
					potentialDestinations.Add(space);
					continue;
				}

				coastSpaces.Add(space);
				foreach (Space adjacency in _adjacencies[space])
				{
					if (!coastSpaces.Contains(adjacency)) spacesToCheck.Enqueue(adjacency);
				}
			}
		}

		Space destination = potentialDestinations[Random.Range(0, potentialDestinations.Count)];
		player.MoveTo(destination.transform);
	}
	#endregion
}
