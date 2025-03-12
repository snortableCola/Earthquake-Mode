using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool IsFrozen;
	
	[SerializeField] private float _movementTime;
	[SerializeField] private float _jumpHeight;
	[SerializeField] private float _movementDelay;

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
			Debug.Log($"{this} was frozen and passed its turn.");
			IsFrozen = false;
			yield break;
		}

		int distance = Random.Range(1, 7);
		Debug.Log($"{this} moves {distance} spaces.");

		Space space = transform.GetComponentInParent<Space>();

		yield return new WaitForSeconds(_movementDelay);

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
		while (timeMoving < _movementTime)
		{
			timeMoving += Time.deltaTime;
			float t = timeMoving / _movementTime;

			Vector3 currentPosition = Vector3.Lerp(start, destination, t);
			currentPosition.y += _jumpHeight * t * (1 - t);

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
