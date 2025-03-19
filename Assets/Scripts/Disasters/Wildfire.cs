using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wildfire : Disaster
{
	[SerializeField] private int _initialFireSize = 3;
	[SerializeField] private int _fireDuration = 3;

	public override bool IsPossible => !_isFireOngoing;

	private bool _isFireOngoing;
	private int _roundsOfFireRemaining;
	private Player _fireStarter;

	private readonly List<Space> _flammableSpaces = new();
	private readonly Queue<Space> _allBurningSpaces = new();
	private readonly Queue<Space> _spacesLastEngulfed = new();

	public override void Refresh()
	{
		_flammableSpaces.Clear();
		_flammableSpaces.AddRange(GameManager.Instance.Spaces.Where(IsFlammable));

		if (!_isFireOngoing) return;

		_spacesLastEngulfed.Clear();
		
		int spacesToRefresh = _allBurningSpaces.Count;
		for (int i = 0; i < spacesToRefresh; i++)
		{
			Space space = _allBurningSpaces.Dequeue();

			if (IsFlammable(space))
			{
				_spacesLastEngulfed.Enqueue(space);
				_allBurningSpaces.Enqueue(space);
			}
			else
			{
				space.BurningTag.State = false;
			}
		}
	}

	private static bool IsFlammable(Space space) => space.Biome is Biome.Plains or Biome.Mountains;

	public override IEnumerator StartDisaster(Player incitingPlayer)
	{
		_isFireOngoing = true;
		_fireStarter = incitingPlayer;
		_roundsOfFireRemaining = _fireDuration;

		// Randomly select the spaces at which the fire will start
		var fireStartingSpaces = _flammableSpaces.OrderBy(_ => Random.value).Take(_initialFireSize);

		// Set the selected spaces on fire
		foreach (Space space in fireStartingSpaces)
		{
			SetSpaceOnFire(space);
		}

		yield break;
	}

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
		int spacesToSpread = _spacesLastEngulfed.Count;
		for (int i = 0; i < spacesToSpread; i++)
		{
			Space spaceToSpread = _spacesLastEngulfed.Dequeue();

			foreach (Adjacency adjacency in AdjacencyManager.Instance.Adjacencies[spaceToSpread])
			{
				Space space = adjacency.Space;

				// Only spread fire to flammable spaces that aren't already on fire
				if (!space.BurningTag.State && IsFlammable(space)) SetSpaceOnFire(space);
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
		while (_allBurningSpaces.Count > 0)
		{
			Space spaceToExtinguish = _allBurningSpaces.Dequeue();
			spaceToExtinguish.BurningTag.State = false;
		}

		_spacesLastEngulfed.Clear();

		Debug.Log("Fire extinguished.");
	}

	/// <summary>
	/// Sets a space on fire and registers it as a newly-inflamed space.
	/// </summary>
	/// <param name="space">The space to set on fire.</param>
	private void SetSpaceOnFire(Space space)
	{
		space.BurningTag.State = true;

		_spacesLastEngulfed.Enqueue(space);
		_allBurningSpaces.Enqueue(space);
	}
}
