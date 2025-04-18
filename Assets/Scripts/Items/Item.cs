using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public abstract IEnumerator BeUsedBy(Player player);
}
