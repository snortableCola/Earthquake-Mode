using System.Collections;

public class TravelPlan : Item
{
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
