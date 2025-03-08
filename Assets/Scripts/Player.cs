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

		Space space = transform.GetComponentInParent<Space>();
		
		while (distance > 0)
		{
			yield return new WaitForSeconds(MovementTime);

			space = space.GetComponent<NextSpaceProvider>().NextSpace;

			if (space.TryGetComponent<SpacePassedBehavior>(out var behavior))
			{
				MoveTo(space, false);
				behavior.ReactToPlayerPassing(this);
				continue;
			}

			MoveTo(space, --distance == 0);
		}
	}

	public void MoveTo(Space space, bool triggerLandingBehavior)
	{
		transform.SetParent(space.transform, false);

		if (!triggerLandingBehavior) return;

		if (space.TryGetComponent<SpaceLandedBehavior>(out var behavior))
		{
			behavior.ReactToPlayerLanding(this);
		}
	}
}
