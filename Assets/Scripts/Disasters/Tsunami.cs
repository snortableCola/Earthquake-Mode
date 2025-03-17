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
				case Biome.Shore:
					StartCoroutine(player.Movement.JumpToSpaceCoroutine(_tsunamiFailsafe, false));
					break;
				case Biome.Mountains:
					player.FrozenTag.State = true;
					break;
			}
		}
	}
}
