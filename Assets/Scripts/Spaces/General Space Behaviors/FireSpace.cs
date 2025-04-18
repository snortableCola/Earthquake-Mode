using System.Collections;
using UnityEngine;

public class FireSpace : MonoBehaviour
{
	public static FireSpace Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public IEnumerator RespondToPlayerEnd(Player player)
	{
		Debug.Log($"{player.name} landed on a space that is on fire.");
		player.Coins -= 5;
		yield break;
	}
}
