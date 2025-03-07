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
		int distance = Random.Range(1, 7);
		Debug.Log($"Moving {distance}");

		NextSpaceProvider nextSpaceProvider = transform.GetComponentInParent<NextSpaceProvider>();
		for (int step = 1; step <= distance; step++)
		{
			yield return new WaitForSeconds(MovementTime);

			Space nextSpace = nextSpaceProvider.NextSpace;
			nextSpaceProvider = nextSpace.GetComponent<NextSpaceProvider>();

			MoveTo(nextSpace, step == distance);
		}
	}

	public void MoveTo(Space space, bool triggerLandingBehavior)
	{
		transform.SetParent(space.transform, false);

		if (!triggerLandingBehavior) return;

		SpaceLandedBehavior behavior = space.GetComponent<SpaceLandedBehavior>();
		if (behavior != null)
		{
			behavior.ReactToPlayerLanding(this);
		}
	}
}
