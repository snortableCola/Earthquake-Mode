using System.Collections;

public class PrivateJet : Item
{
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
