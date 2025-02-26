using UnityEngine;

public class LinearNextSpaceProvider : NextSpaceProvider
{
	[SerializeField] private Space _nextSpace;
	public override Space NextSpace => _nextSpace;
}
