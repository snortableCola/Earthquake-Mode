using UnityEngine;

public class PrototypePlayer : MonoBehaviour
{
	public bool IsFrozen;

	public PrototypeSpace.BoardRegion CurrentRegion => transform.GetComponentInParent<PrototypeSpace>().Region;

	[ContextMenu("Pass Turn")]
	public void PassTurn()
	{
		if (IsFrozen)
		{
			Debug.Log("Was frozen!");
			IsFrozen = false;
		}
		else
		{
			Debug.Log("Took turn!");
		}
	}
}
