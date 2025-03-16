using UnityEngine;

public abstract class SpaceBehavior : MonoBehaviour
{
	public abstract bool EndsTurn { get; }

	public abstract void RespondToPlayer(Player player);
}
