using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool IsFrozen;
	
	public float MovementTime;
	public float JumpHeight;

	public int totalPoints = 100; // Player's total points

    // Method to adjust points
    public void AdjustPoints(int amount)
    {
        totalPoints += amount;
    }
    public Space.BoardBiome CurrentBiome => transform.GetComponentInParent<Space>().Biome;

	[ContextMenu("Move")]
	public void Move() => StartCoroutine(MovementCoroutine());

	public IEnumerator MovementCoroutine()
	{
		if (IsFrozen)
		{
			Debug.Log($"{this} could not move, was frozen!");
			IsFrozen = false;
			yield break;
		}

		int distance = Random.Range(1, 7);
		Debug.Log($"Moving {distance}");

		Space space = transform.GetComponentInParent<Space>();

		yield return new WaitForSeconds(0.5f);

		while (distance > 0)
		{
			space = space.GetComponent<NextSpaceProvider>().NextSpace;

			if (space.TryGetComponent<SpacePassedBehavior>(out var behavior))
			{
				yield return MoveTo(space, false);
				behavior.ReactToPlayerPassing(this);
				continue;
			}

			yield return MoveTo(space, --distance == 0);
		}
	}

	public IEnumerator MoveTo(Space space, bool triggerLandingBehavior)
	{
		Vector3 start = transform.position;
		Vector3 destination = space.transform.TransformPoint(transform.localPosition);

		float timeMoving = 0;
		while (timeMoving < MovementTime)
		{
			timeMoving += Time.deltaTime;
			float t = timeMoving / MovementTime;

			Vector3 currentPosition = Vector3.Lerp(start, destination, t);
			currentPosition.y += JumpHeight * t * (1 - t);

			transform.position = currentPosition;

			yield return null;
		}

		transform.position = destination;
		transform.SetParent(space.transform);

		if (!triggerLandingBehavior) yield break;

		if (space.TryGetComponent<SpaceLandedBehavior>(out var behavior))
		{
			behavior.ReactToPlayerLanding(this);
		}
	}
}
