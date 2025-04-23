using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public int cost;
	public Sprite itemSprite;
	public string itemName; 
	public abstract IEnumerator BeUsedBy(Player player);
}
