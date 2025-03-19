using System.Collections;
using UnityEngine;

public class Earthquake : Disaster
{
	private SpaceSwapping[] _spacesToSwap;

	private void Awake()
	{
		_spacesToSwap = FindObjectsByType<SpaceSwapping>(FindObjectsSortMode.None);
	}

	public override IEnumerator StartDisaster(Player incitingPlayer)
	{
		foreach (var space in _spacesToSwap)
		{
			space.PerformSwap();
		}
		DisasterManager.Instance.RefreshDisasters();
		yield break;
	}
}
