using System.Collections;
using UnityEngine;

public class Tsunami : Disaster
{
	[SerializeField] private Space _tsunamiFailsafe;

	public override IEnumerator StartDisaster(Player _)
	{
		foreach (Player player in GameManager.Instance.Players)
		{
			switch (player.CurrentBiome)
			{
				case Biome.Shore:
					yield return player.Movement.JumpToSpaceCoroutine(_tsunamiFailsafe);
					break;
				case Biome.Mountains:
					player.FrozenTag.State = true;
					break;
			}
		}
	}
}
