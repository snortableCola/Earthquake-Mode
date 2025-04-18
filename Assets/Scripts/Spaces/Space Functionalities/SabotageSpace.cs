using System.Collections;
using UnityEngine;

public class SabotageSpace : SpaceBehavior
{
	public override IEnumerator RespondToPlayerEnd(Player player) => SabotageManager.Instance.Sabotage(player);
}
