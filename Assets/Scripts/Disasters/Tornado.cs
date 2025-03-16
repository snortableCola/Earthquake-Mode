using System.Collections.Generic;
using UnityEngine;

public class Tornado : Disaster
{
	private readonly List<Space> potentialDestinations = new();

	public override bool IsPossible { get; } = true;

	private void Start()
	{
		RefreshSpaces();
	}

	public void RefreshSpaces()
	{
		potentialDestinations.Clear();
		foreach (Space space in GameManager.Instance.Spaces)
		{
			if (space.GetComponent<NegativeSpace>() && space.Biome != Biome.Plains) potentialDestinations.Add(space);
		}
	}

	public override void StartDisaster(Player _)
	{
		int potentialLandingsCount = potentialDestinations.Count;

		// Iterate through all players
		foreach (Player player in GameManager.Instance.Players)
		{
			// Only blow them away if they're in the plains
			if (player.CurrentBiome != Biome.Plains) continue;

			// Select a random negative, non-plains space to plow the player
			int chosenLandingIndex = Random.Range(0, potentialLandingsCount);
			Space landingSpace = potentialDestinations[chosenLandingIndex];

			// Move the player to the selected space, triggering the negative behavior
			StartCoroutine(player.Movement.JumpToSpaceCoroutine(landingSpace, true));
		}
	}
}
