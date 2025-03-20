using System.Collections;
using UnityEngine;

public abstract class SpaceBehavior : MonoBehaviour
{
	public virtual bool EndsTurn { get; } = true;

	public abstract IEnumerator RespondToPlayer(Player player);
}
