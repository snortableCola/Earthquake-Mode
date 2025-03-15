using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Space.BoardBiome;

public class DisasterManager : MonoBehaviour
{
	private Player[] _players;
	[SerializeField] private Transform _board;
	[SerializeField] private int _disasterThreshold;
	[SerializeField] private AdjacencyManager _adjacencyManager;

	private void Awake()
	{
		Space[] allSpaces = _board.GetComponentsInChildren<Space>();
		_players = _board.GetComponentsInChildren<Player>();

		foreach (Space space in allSpaces)
		{
			if (space.TryGetComponent<NegativeSpace>(out _) && space.Biome != Plains) _tornadoLandingSpaces.Add(space);

			if (space.Biome is Plains or Mountains) _flammableSpaces.Add(space);
		}
	}

	#region Disaster-Triggering Logic

	private readonly Dictionary<DisasterType, int> _disasterTracker = new()
	{
		{DisasterType.Earthquake, 0},
		{DisasterType.Wildfire, 0},
		{DisasterType.Tsunami, 0},
		{DisasterType.Tornado, 0}
	};

	/// <summary>
	/// Increments the disaster level of whatever is associated with the specified biome.
	/// </summary>
	/// <param name="biome">The biome whose corresponding disaster should be incremented.</param>
	/// <param name="player">The player responsible for the disaster level's incrementation.</param>
	public void IncrementBiomeDisaster(Space.BoardBiome biome, Player player)
	{
		DisasterType disaster;

		switch (biome)
		{
			case Coast:
				disaster = DisasterType.Tsunami;
				break;
			case Plains:
				disaster = DisasterType.Tornado;
				break;
			case Mountains:
				if (_isFireOngoing) goto default; // Fire level cannot be incremented if there's already an ongoing fire.
				disaster = DisasterType.Wildfire;
				break;
			default:
				Debug.Log("Disaster level not incremented.");
				return;
		}

		IncrementDisaster(disaster, player);
	}

	/// <summary>
	/// Increments the disaster level for a specified disaster, triggering it if it reaches the disaster threshold.
	/// </summary>
	/// <param name="disaster">The disaster to increment the disaster level of, and to potentially trigger.</param>
	/// <param name="incitingPlayer"></param>
	public void IncrementDisaster(DisasterType disaster, Player incitingPlayer)
	{
		int disasterLevel = ++_disasterTracker[disaster];

		Debug.Log($"{disaster} at level {disasterLevel}");
		if (disasterLevel != _disasterThreshold) return;

		Debug.Log($"{disaster} starting.");

		switch (disaster)
		{
			case DisasterType.Tsunami:
				DoTsunamiDisaster();
				break;
			case DisasterType.Tornado:
				DoTornadoDisaster();
				break;
			case DisasterType.Wildfire:
				StartFireDisaster(incitingPlayer);
				break;
		}

		_disasterTracker[disaster] = 0; // Reset the disaster level once startign the disaster
	}
	#endregion

	#region Fire Logic
	[SerializeField] private int _initialFireSize = 3;
	[SerializeField] private int _fireDuration = 3;

	private bool _isFireOngoing;
	private int _roundsOfFireRemaining;
	private Player _fireStarter;

	private readonly List<Space> _flammableSpaces = new();
	private readonly Queue<Space> _spacesToExtinguish = new();
	private readonly Queue<Space> _spacesSetOnFire = new();

	/// <summary>
	/// Starts the fire disaster by starting fires at randomly-selected field and mountain tiles.
	/// </summary>
	public void StartFireDisaster(Player fireStarter)
	{
		_isFireOngoing = true;
		_fireStarter = fireStarter;

		_roundsOfFireRemaining = _fireDuration;

		// Randomly select the spaces at which the fire will start
		var fireStartingSpaces = _flammableSpaces.OrderBy(_ => Random.value).Take(_initialFireSize);

		// Set the selected spaces on fire
		foreach (Space space in fireStartingSpaces)
		{
			SetSpaceOnFire(space);
		}
	}

	/// <summary>
	/// Tries to progress a round of fire, either spreading it or extinguishing it depending on the rounds remaining.
	/// </summary>
	public void TryFireProgress(Player player)
	{
		// Fire can only progress if there is a fire, it's the fire starter's turn, and the fire hasn't just started.
		if (!_isFireOngoing || player != _fireStarter || _roundsOfFireRemaining-- == _fireDuration) return;

		if (_roundsOfFireRemaining >= 0) SpreadFire();
		else EndFireDisaster();
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

			foreach (Adjacency adjacency in _adjacencyManager.Adjacencies[spaceToSpread])
			{
				// Only spread fire to flammable spaces that aren't already on fire
				if (!adjacency.Space.BurningTag.State && adjacency.Space.Biome is Mountains or Plains) SetSpaceOnFire(adjacency.Space);
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
			spaceToExtinguish.BurningTag.State = false;
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
		space.BurningTag.State = true;

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
	public void DoTornadoDisaster()
	{
		int potentialLandingsCount = _tornadoLandingSpaces.Count;

		// Iterate through all players
		foreach (Player player in _players)
		{
			// Only blow them away if they're in the plains
			if (player.CurrentBiome != Plains) continue;

			// Select a random negative, non-plains space to plow the player
			int chosenLandingIndex = Random.Range(0, potentialLandingsCount);
			Space landingSpace = _tornadoLandingSpaces[chosenLandingIndex];

			// Move the player to the selected space, triggering the negative behavior
			StartCoroutine(player.JumpToSpaceCoroutine(landingSpace, true));
		}
	}
	#endregion

	#region Tsunami Logic
	[SerializeField] private Space _tsunamiFailsafe;

	/// <summary>
	/// Blows all players occupying coast spaces onto a predetermined space, and freezes all players occupying mountains spaces.
	/// </summary>
	public void DoTsunamiDisaster()
	{
		foreach (Player player in _players)
		{
			switch (player.CurrentBiome)
			{
				case Coast:
					TsunamiPush(player);
					break;
				case Mountains:
					player.FrozenTag.State = true;
					break;
			}
		}
	}

	/// <summary>
	/// Pushes the specified player to their closest non-shore space, or a failsafe space if one could not be found.
	/// </summary>
	/// <param name="player">The player to push.</param>
	private void TsunamiPush(Player player)
	{
		Space startingSpace = player.GetComponentInParent<Space>();

		// All of this is just breadth-first search
		HashSet<Space> coastSpaces = new() { startingSpace };
		Queue<Adjacency> spacesToCheck = new(_adjacencyManager.Adjacencies[startingSpace]);

		List<Space> potentialDestinations = new();
		while (potentialDestinations.Count == 0 && spacesToCheck.Count != 0)
		{
			int checks = spacesToCheck.Count;
			for (int i = 0; i < checks; i++)
			{
				Space space = spacesToCheck.Dequeue().Space;

				if (space.Biome is not Coast)
				{
					potentialDestinations.Add(space);
					continue;
				}

				coastSpaces.Add(space);
				foreach (Adjacency adjacency in _adjacencyManager.Adjacencies[space])
				{
					if (!coastSpaces.Contains(adjacency.Space)) spacesToCheck.Enqueue(adjacency);
				}
			}
		}

		// Randomly selects from all possible spaces found, or uses the failsafe space if none could be found.
		Space destination = _tsunamiFailsafe;
		if (potentialDestinations.Count > 0) destination = potentialDestinations[Random.Range(0, potentialDestinations.Count)];

		StartCoroutine(player.JumpToSpaceCoroutine(destination, false));
	}
	#endregion

	public enum DisasterType
	{
		Wildfire,
		Tornado,
		Tsunami,
		Earthquake
	}
}
