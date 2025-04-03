using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
	public static DisasterManager Instance { get; private set; }

	[SerializeField] private int _disasterThreshold;

	[SerializeField] private Tsunami _tsunami;
	[SerializeField] private Tornado _tornado;
	[SerializeField] private Wildfire _wildfire;
	[SerializeField] private Earthquake _earthquake;

	private void Awake()
	{
		Instance = this;
		_disasterTracker = new()
		{
			{_wildfire, 0},
			{_tsunami, 0},
			{_tornado, 0},
			{_earthquake, 0}
		};
	}

	private Dictionary<Disaster, int> _disasterTracker;

	/// <summary>
	/// Increments the disaster level of whatever is associated with the specified biome.
	/// </summary>
	/// <param name="biome">The biome whose corresponding disaster should be incremented.</param>
	/// <param name="player">The player responsible for the disaster level's incrementation.</param>
	public IEnumerator IncrementBiomeDisaster(Biome biome, Player player)
	{
		Disaster disaster;

		switch (biome)
		{
			case Biome.Shore:
				disaster = _tsunami;
				break;
			case Biome.Plains:
				disaster = _tornado;
				break;
			case Biome.Mountains:
				disaster = _wildfire;
				break;
			default: yield break;
		}

		yield return IncrementDisaster(disaster, player);
	}

	public IEnumerator IncrementEarthquake() => IncrementDisaster(_earthquake, null);

	/// <summary>
	/// Increments the disaster level for a specified disaster, triggering it if it reaches the disaster threshold.
	/// </summary>
	/// <param name="disaster">The disaster to increment the disaster level of, and to potentially trigger.</param>
	/// <param name="incitingPlayer"></param>
	private IEnumerator IncrementDisaster(Disaster disaster, Player incitingPlayer)
	{
		if (!disaster.IsPossible) yield break;

		int disasterLevel = ++_disasterTracker[disaster];
		Debug.Log($"{disaster.name} at level {disasterLevel}");

		if (disasterLevel != _disasterThreshold) yield break;
		yield return disaster.StartDisaster(incitingPlayer);
		_disasterTracker[disaster] = 0; // Reset the disaster level once starting the disaster
	}

	public void RefreshDisasters()
	{
		foreach(Disaster disaster in _disasterTracker.Keys)
		{
			disaster.Refresh();
		}
	}
}
