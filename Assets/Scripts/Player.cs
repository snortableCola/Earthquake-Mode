using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool IsFrozen;
	public float MovementTime;
	public int totalPoints = 100; // Player's total points

    // Method to adjust points
    public void AdjustPoints(int amount)
    {
        totalPoints += amount;
    }
    public Space.BoardBiome CurrentRegion => transform.GetComponentInParent<Space>().Biome;

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
	public void Move() => StartCoroutine(MovementCorutine());

	public IEnumerator MovementCorutine()
	{
		NextSpaceProvider current = transform.GetComponentInParent<NextSpaceProvider>();
		int distance = Random.Range(1, 7);
		Debug.Log($"Moving {distance}");

		for (int i = 0; i < distance; i++)
		{
			yield return new WaitForSeconds(MovementTime);

			current = current.NextSpace.GetComponent<NextSpaceProvider>();
			MoveTo(current.transform);
		}
	}

	public void MoveTo(Transform space)
	{
		transform.SetParent(space, false);
	}
}
