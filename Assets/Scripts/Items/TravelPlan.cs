using System.Collections;

public class TravelPlan : Item
{
	private void Awake()
	{
		cost = 5;
		itemName = "Travel Plan"; 
	}
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
