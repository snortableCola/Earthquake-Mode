using System.Collections;

public class PrivateJet : Item
{
	private void Awake()
	{
		cost = 10;
		itemName = "Private Jet";
	}
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
