using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationFinder : MonoBehaviour
{
	public static DestinationFinder Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public Space[] GetPossibleDestinations(Space space, int distance)
	{
		var adjacencies = AdjacencyManager.Instance.Adjacencies;

		HashSet<Space> results = new();
		HashSet<(Space, int)> visited = new() { (space, 0) };
		Queue<(Space, int)> queue = new(visited);

		while (queue.Count > 0)
		{
			(Space current, int cost) = queue.Dequeue();

			if (cost == distance)
			{
				results.Add(current);
				continue;
			}

			foreach (Adjacency adj in adjacencies[current])
			{
				if (!adj.IsForwards) continue;

				Space next = adj.Space;
				int newCost = cost;
				newCost++;

				if (visited.Add((next, newCost))) queue.Enqueue((next, newCost));

				if (next.Behavior is not TransportSpace transport) continue;
				next = transport.Destination;

				if (visited.Add((next, newCost))) queue.Enqueue((next, newCost));
			}
		}

		return results.ToArray();
	}
}
