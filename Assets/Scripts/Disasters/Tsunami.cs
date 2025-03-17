using System.Collections.Generic;
using UnityEngine;

public class Tsunami : Disaster
{
	[SerializeField] private Space _tsunamiFailsafe;

	public override bool IsPossible { get; } = true;

	public override void Refresh() { }

	public override void StartDisaster(Player _)
	{
		foreach (Player player in GameManager.Instance.Players)
		{
			switch (player.CurrentBiome)
			{
				case Biome.Coast:
					PushAway(player);
					break;
				case Biome.Mountains:
					player.FrozenTag.State = true;
					break;
			}
		}
	}

	private void PushAway(Player player)
	{
		Space startingSpace = player.GetComponentInParent<Space>();

		// All of this is just breadth-first search
		HashSet<Space> coastSpaces = new() { startingSpace };
		Queue<Adjacency> spacesToCheck = new(AdjacencyManager.Instance.Adjacencies[startingSpace]);

		List<Space> potentialDestinations = new();
		while (potentialDestinations.Count == 0 && spacesToCheck.Count != 0)
		{
			int checks = spacesToCheck.Count;
			for (int i = 0; i < checks; i++)
			{
				Space space = spacesToCheck.Dequeue().Space;

				if (space.Biome is not Biome.Coast)
				{
					potentialDestinations.Add(space);
					continue;
				}

				coastSpaces.Add(space);
				foreach (Adjacency adjacency in AdjacencyManager.Instance.Adjacencies[space])
				{
					if (!coastSpaces.Contains(adjacency.Space)) spacesToCheck.Enqueue(adjacency);
				}
			}
		}

		// Randomly selects from all possible spaces found, or uses the failsafe space if none could be found.
		Space destination = _tsunamiFailsafe;
		if (potentialDestinations.Count > 0) destination = potentialDestinations[Random.Range(0, potentialDestinations.Count)];

		StartCoroutine(player.Movement.JumpToSpaceCoroutine(destination, false));
	}
}
