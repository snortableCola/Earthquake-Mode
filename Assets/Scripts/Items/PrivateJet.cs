using System.Collections;
using UnityEngine;

public class PrivateJet : MonoBehaviour, IItem
{
	public IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;
		yield break;
	}
}
