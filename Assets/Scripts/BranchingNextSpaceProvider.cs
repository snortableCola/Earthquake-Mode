using UnityEngine;

public class BranchingNextSpaceProvider : NextSpaceProvider
{
	[SerializeField] private Space[] _nextSpaces;
	public override Space NextSpace => _nextSpaces[Random.Range(0, _nextSpaces.Length)];
}
