using System.Collections;
using UnityEngine;

public abstract class SpaceBehavior : MonoBehaviour
{
	public abstract bool EndsTurn { get; }

	public abstract IEnumerator RespondToPlayer(Player player);
}
