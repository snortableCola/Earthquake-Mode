using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool IsFrozen;
	public float MovementTime;

	public Space.BoardRegion CurrentRegion => transform.GetComponentInParent<Space>().Region;

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

	[ContextMenu("Move")]
	public void Move()
	{
		StartCoroutine(MovementCorutine());
	}

	public IEnumerator MovementCorutine()
	{
		Space current = transform.GetComponentInParent<Space>();
		int distance = Random.Range(1, 7);
		Debug.Log($"Moving {distance}");

		for (int i = 0; i < distance; i++)
		{
			yield return new WaitForSeconds(MovementTime);

			current = current.NextSpace;
			transform.SetParent(current.transform, false);
		}
	}
}
