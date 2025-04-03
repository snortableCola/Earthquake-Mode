using System.Collections;
using UnityEngine;

public class HeliEvac : MonoBehaviour, IItem
{
	public IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
