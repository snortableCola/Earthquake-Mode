using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class SpaceSwapping : MonoBehaviour
{
	private Space _thisSpace;
	public Space OtherSpace;

	private void Awake()
	{
		_thisSpace = GetComponent<Space>();
	}

	[ContextMenu("SWAP! SWAP!")]
	public void PerformSwap()
    {
		Dictionary<Space, List<Adjacency>> allAdjacencies = AdjacencyManager.Instance.Adjacencies;

		List<Adjacency> myAdjacencies = allAdjacencies[_thisSpace];
		Biome myBiome = _thisSpace.Biome;
		Vector3 myPosition = transform.position;

		List<Adjacency> theirAdjacencies = allAdjacencies[OtherSpace];
		Biome theirBiome = OtherSpace.Biome;
		Vector3 theirPosition = OtherSpace.transform.position;

		// Move them to me

		allAdjacencies[OtherSpace] = myAdjacencies; // They now point to my adjacencies
		foreach (Adjacency myAdjacency in myAdjacencies)
		{
			List<Adjacency> adjacenciesOfMyAdjacency = allAdjacencies[myAdjacency.Space];
			Adjacency pointerToMe = adjacenciesOfMyAdjacency.First(adj => adj.Space == _thisSpace);
			pointerToMe.Space = OtherSpace; // My adjacencies now point to them instead of me
		}
		OtherSpace.Biome = myBiome; // They've now taken my biome
		OtherSpace.transform.position = myPosition; // They've now taken my position


		// Move me to them

		allAdjacencies[_thisSpace] = theirAdjacencies;
		foreach (Adjacency theirAdjacency in theirAdjacencies)
		{
			List<Adjacency> adjacenciesOfTheirAdjacency = allAdjacencies[theirAdjacency.Space];
			Adjacency pointerToThem = adjacenciesOfTheirAdjacency.First(adj => adj.Space == OtherSpace);
			pointerToThem.Space = _thisSpace;
		}
		_thisSpace.Biome = theirBiome;
		transform.position = theirPosition;
    }
}
