using System.Collections;

public class HeliEvac : Item
{
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
