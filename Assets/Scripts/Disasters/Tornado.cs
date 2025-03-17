using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tornado : Disaster
{
	private readonly List<Space> _potentialDestinations = new();

	public override bool IsPossible { get; } = true;

	public override void Refresh()
	{
		_potentialDestinations.Clear();
		_potentialDestinations.AddRange(GameManager.Instance.Spaces.Where(IsValidTornadoDestination));
	}

	private static bool IsValidTornadoDestination(Space space) => space.Biome != Biome.Plains && space.GetComponent<NegativeSpace>();

	public override void StartDisaster(Player _)
	{
		var victims = GameManager.Instance.Players.Where(IsValidTornadoVictim).GetEnumerator();
		var destinations = _potentialDestinations.OrderBy(_ => Random.value).GetEnumerator();

		while (victims.MoveNext() && destinations.MoveNext())
		{
			StartCoroutine(victims.Current.Movement.JumpToSpaceCoroutine(destinations.Current, true));
		};
	}

	private static bool IsValidTornadoVictim(Player player) => player.CurrentBiome == Biome.Plains;
}
