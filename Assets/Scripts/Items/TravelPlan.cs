using System.Collections;
using UnityEngine;

public class TravelPlan : MonoBehaviour, IItem
{
	public IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
