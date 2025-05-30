using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdjacencyManager : MonoBehaviour
{
	public static AdjacencyManager Instance { get; private set; }

	public Dictionary<Space, List<Adjacency>> Adjacencies;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Space[] allSpaces = GameManager.Instance.Spaces;
		Adjacencies = allSpaces.ToDictionary(space => space, _ => new List<Adjacency>());

		foreach (Space origin in allSpaces)
		{
			Vector3 originPosition = origin.transform.position;

			Space[] destinations = origin.GetComponent<NextSpaceProvider>().NextSpaces;
			foreach (Space destination in destinations)
			{
				Vector3 destinationPosition = destination.transform.position;

				float dx = destinationPosition.x - originPosition.x;
				float dy = destinationPosition.z - originPosition.z;

				Vector2 direction = new Vector2(dx, dy).normalized;

				Adjacencies[origin].Add(new Adjacency(direction, destination, true));
				Adjacencies[destination].Add(new Adjacency(-direction, origin, false));
			}
		}
	}
}