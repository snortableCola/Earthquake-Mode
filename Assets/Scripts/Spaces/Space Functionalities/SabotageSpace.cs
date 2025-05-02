using System.Collections;

public class SabotageSpace : SpaceBehavior
{
	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		if (SabotageManager.Instance.Possible)
		{
			yield return SabotageManager.Instance.Sabotage(player);
		}
		else
		{
			yield return GeneralResourceSpace.Instance.RespondToPlayerEnd(player);
		}
	}
}
